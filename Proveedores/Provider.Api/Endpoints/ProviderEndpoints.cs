using Provider.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Provider.Application.UseCases.Abstractions.UseCases;

namespace Provider.Api.Endpoints
{
    public static class ProviderEndpoints
    {
        public static void MapProviderEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/providers")
                           .WithTags("Proveedores");

            // --- 1. GET ALL ---
            group.MapGet("/", async (
                IInteractorUseCase<object?, IEnumerable<ProviderModel.ProviderData>> useCase,
                CancellationToken ct) =>
            {
                // Pasamos "admin" como usuario por ahora, o podrías obtenerlo del HttpContext
                var response = await useCase.InteractAsync("admin", null, ct);
                return Results.Ok(response);
            });

            // --- 2. CREATE ---
            group.MapPost("/", async (
                IInteractorUseCase<ProviderModel.CreateProvider, bool> useCase,
                [FromBody] ProviderModel.CreateProvider command,
                CancellationToken ct) =>
            {
                var response = await useCase.InteractAsync("admin", command, ct);
                return response
                    ? Results.Created($"/api/providers/{command.Item.Id}", command.Item)
                    : Results.BadRequest("No se pudo crear el proveedor");
            });

            // --- 3. UPDATE ---
            group.MapPut("/", async (
                IInteractorUseCase<ProviderModel.UpdateProvider, bool> useCase,
                [FromBody] ProviderModel.UpdateProvider command,
                CancellationToken ct) =>
            {
                var response = await useCase.InteractAsync("admin", command, ct);
                return response ? Results.NoContent() : Results.NotFound();
            });

            // --- 4. DELETE ---
            group.MapDelete("/{id:guid}", async (
                IInteractorUseCase<Guid, bool> useCase,
                Guid id,
                CancellationToken ct) =>
            {
                var response = await useCase.InteractAsync("admin", id, ct);
                return response ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}