using Catalog.Application.Mappers.CategoryMappers;
using Catalog.Application.Queries.CategoryQueries;
using Catalog.Application.Responses.CategoryResponses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.CategoryHandler;

public class GetCategoriesAllHandler : IRequestHandler<GetCategoriesAllQuery, IEnumerable<CategoryResponse>>
{

    private readonly ICategoryRepository _categoryRepository;


    public GetCategoriesAllHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesAllQuery request, CancellationToken cancellationToken)
    {
        var query = await _categoryRepository.GetCategoriesAll();
        var response = CategoryMapper.Mapper.Map<IEnumerable<CategoryResponse>>(query);
        return response;
    }
}
