using Catalog.Api.Endpoints;
using Catalog.Application.Handlers.CategoryHandler;
using Catalog.Application.Queries.CategoryQueries;
using Catalog.Core.Repositories;
using Catalog.Core.Repositories.Base;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Repositories.Base;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ESCUCHA ---
builder.WebHost.UseUrls("http://0.0.0.0:5071");

// --- 2. CONSUL ---
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    // ✅ Nombre del servicio en docker-compose, no localhost
    cfg.Address = new Uri("http://consul-server:8500");
}));

// --- 3. BASE DE DATOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseNpgsql(connectionString));

// --- 4. REPOSITORIOS ---
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// --- 5. MEDIATR, VALIDATORS, AUTOMAPPER ---
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetCategoriesAllHandler).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(GetCategoriesAllHandler).Assembly);
builder.Services.AddAutoMapper(typeof(GetCategoriesAllQuery).Assembly);

// --- 6. SERVICIOS ---
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
    //.AddNpgSql(connectionString!); // ✅ health check de postgres

builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:8000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// --- 7. REGISTRO EN CONSUL ---
var lifetime = app.Lifetime;
var consulClient = app.Services.GetRequiredService<IConsulClient>();
const string serviceId = "catalog-api";

lifetime.ApplicationStarted.Register(async () =>
{
    Console.WriteLine("--> REGISTRANDO CATALOG EN CONSUL...");
    var registration = new AgentServiceRegistration
    {
        ID = serviceId,
        Name = "catalog-service",
        Address = "catalog-api",  // ✅ nombre del contenedor en docker-compose
        Port = 5071,
        Check = new AgentServiceCheck
        {
            HTTP = "http://catalog-api:5071/health",
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
    Console.WriteLine("--> DESREGISTRANDO CATALOG DE CONSUL...");
    await consulClient.Agent.ServiceDeregister(serviceId);
});

// =============================================================
// --- 8. PIPELINE ---
// =============================================================

// ✅ PathBase primero
app.UsePathBase("/catalog");

app.UseCors("GatewayPolicy");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/v1.json";
        options.Servers = new[]
        {
            new ScalarServer("http://localhost:8000/catalog")
        };
    });
}

app.UseAuthorization();
app.MapCategoryEndpoints();

app.Run();