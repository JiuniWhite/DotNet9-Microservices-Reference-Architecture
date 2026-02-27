using Catalog.Application.Responses.CategoryResponses;
using MediatR;

namespace Catalog.Application.Queries.CategoryQueries
{
    public class GetCategoriesAllQuery : IRequest<IEnumerable<CategoryResponse>>
    {
        public GetCategoriesAllQuery() { }
    }
}
