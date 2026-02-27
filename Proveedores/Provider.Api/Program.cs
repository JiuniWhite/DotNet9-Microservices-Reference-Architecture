using Consul;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Provider.Api.Endpoints;
using Provider.Api.Infrastructures.Databases.Context;
using Provider.Application.UseCases.Abstractions.UseCases;
using Provider.Application.UseCases.ProviderUseCases;
using Provider.Infrastructure.Data;
using Provider.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ESCUCHA ---
builder.WebHost.UseUrls("http://0.0.0.0:5039");

// --- 2. CONSUL ---
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri("http://consul-server:8500");
}));

// --- 3. SERVICIOS ---
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IMongoRepository, MongoRepository>();

builder.Services.Scan(selector => selector
    .FromAssemblies(typeof(CreateProviderUseCase).Assembly)
    .AddClasses(classes => classes.AssignableTo(typeof(IInteractorUseCase<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5207")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// --- 4. REGISTRO EN CONSUL ---
var lifetime = app.Lifetime;
var consulClient = app.Services.GetRequiredService<IConsulClient>();
const string serviceId = "provider-api";

lifetime.ApplicationStarted.Register(async () =>
{
    Console.WriteLine("--> REGISTRANDO SERVICIO EN CONSUL...");

    var registration = new AgentServiceRegistration
    {
        ID = serviceId,
        Name = "provider-service",
        Address = "provider-api",   // nombre del servicio en docker-compose
        Port = 5039,
        Check = new AgentServiceCheck
        {
            // Consul golpea directo al contenedor (sin pasar por YARP),
            // por eso la ruta NO lleva el prefijo /providers
            HTTP = "http://provider-api:5039/health",
            Interval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(5)
        }
    };

    try
    {
        await consulClient.Agent.ServiceRegister(registration);
        Console.WriteLine("--> REGISTRO EXITOSO");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> ERROR REGISTRANDO: {ex.Message}");
    }
});

lifetime.ApplicationStopping.Register(async () =>
{
    Console.WriteLine("--> DESREGISTRANDO SERVICIO DE CONSUL...");
    await consulClient.Agent.ServiceDeregister(serviceId);
    Console.WriteLine("--> DESREGISTRO EXITOSO");
});

// =============================================================
// --- 5. PIPELINE  (el orden aquí es crítico) ---
// =============================================================

// ✅ 1º — PathBase: debe ser ABSOLUTAMENTE lo primero.
//    Le indica a todo el pipeline (incluido Scalar) que el prefijo
//    externo es /providers, para que genere sus assets/links correctos.
app.UsePathBase("/providers");

// ✅ 2º — CORS
app.UseCors("GatewayPolicy");

// ✅ 3º — Health check
//    - Consul llama:  http://provider-api:5039/health          → match directo
//    - YARP llama:    http://provider-api:5039/providers/health → UsePathBase
//                     hace el strip y queda /health             → match igual
//    Con una sola definición cubre los dos casos.
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ✅ 4º — Scalar + OpenAPI (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    // Expone el JSON en /providers/openapi/v1.json (visto desde YARP)
    app.MapOpenApi();

    // Expone la UI en /providers/scalar/v1 (visto desde YARP)
    // Gracias a UsePathBase, Scalar genera sus assets con el prefijo
    // correcto y el navegador los encuentra sin 404.
    //app.MapScalarApiReference(options =>
    //{
    //    options.OpenApiRoutePattern = "/openapi/v1.json";

    //});

    app.MapScalarApiReference(options =>
    {
        // ✅ URL absoluta que ve el navegador, no relativa
        options.OpenApiRoutePattern = "/openapi/v1.json";
        options.Servers = new[]
        {
            new ScalarServer("http://localhost:8000/providers")
        };
    });
}

// ✅ 5º — Autorización y endpoints de negocio
app.UseAuthorization();
app.MapProviderEndpoints();

app.Run();