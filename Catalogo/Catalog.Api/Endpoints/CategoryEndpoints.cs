using Catalog.Application.Commands;
using Catalog.Application.Queries.CategoryQueries;
using MediatR;

namespace Catalog.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/categories", async (IMediator mediator) =>
            {
                var query = new GetCategoriesAllQuery();
                var response = await mediator.Send(query);
                return Results.Ok(response);
            }).WithTags("Categoría"); ;

           

            app.MapPost("/api/categories", async (IMediator mediator, CreateCategoryCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Created($"/api/categories/{response}", response);
            })
          .WithTags("Categoría");



            //************************UPDATE**************************************            
            app.MapPut("/api/categories", async (IMediator mediator, UpdateCategoryCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.NoContent();
            })
            .WithTags("Categoría");
        }
    }
}
