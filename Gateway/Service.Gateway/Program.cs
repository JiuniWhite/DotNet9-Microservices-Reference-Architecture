using System.Threading.RateLimiting;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE ENTORNO ---
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true);
}

// --- 2. CONSUL ---
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    var consulHost = Environment.GetEnvironmentVariable("CONSUL_URL") ?? "http://localhost:8500";
    cfg.Address = new Uri(consulHost);
}));

// --- 3. HEALTH CHECKS ---
builder.Services.AddHealthChecks();

// ✅ HealthChecksUI simplificado y corregido
// El problema anterior: intentaba leer la config de YARP que viene de variables
// de entorno, pero en ese punto aún no están disponibles como sección tipada.
// Solución: apuntamos directamente a los contenedores por su nombre DNS interno.

builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddHealthCheckEndpoint("provider-service", "http://provider-api:5039/health");
    setup.AddHealthCheckEndpoint("catalog-service", "http://catalog-api:5071/health"); // ✅
    setup.SetEvaluationTimeInSeconds(25);
})
.AddInMemoryStorage();


// --- 4. RATE LIMITING ---
builder.Services.AddRateLimiter(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
            _ => RateLimitPartition.GetNoLimiter("dev"));
    }
    else
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        {
            var clientIp = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 10
            });
        });
    }

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.Append(
                "Retry-After",
                ((int)retryAfter.TotalSeconds).ToString());
        }

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Demasiadas peticiones",
            message = "Has excedido el límite permitido. Intenta de nuevo en un momento."
        }, token);
    };
});

// --- 5. YARP ---
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((context, handler) =>
    {
        handler.PooledConnectionLifetime = TimeSpan.FromSeconds(30);
        handler.PooledConnectionIdleTimeout = TimeSpan.FromSeconds(10);
    });
// --- 6. CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://acme.sp.com.pe:4200",
                "https://acme.sp.com.pe:4200"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// =============================================================
// --- 7. PIPELINE (el orden es crítico) ---
// =============================================================

// Manejador de errores transitorios — siempre primero
app.Use(async (context, next) =>
{
    try { await next(); }
    catch (Exception)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 503;
            await context.Response.WriteAsync("Gateway: Service temporarily unavailable.");
        }
    }
});

app.UseCors("AngularApp");
app.UseRateLimiter();

// Health checks del propio gateway
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ✅ UI accesible en http://localhost:8000/healthchecks-ui
app.MapHealthChecksUI();

// YARP — siempre al final
app.MapReverseProxy();

app.Run();