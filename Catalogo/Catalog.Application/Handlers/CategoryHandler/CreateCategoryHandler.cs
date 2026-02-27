using Catalog.Application.Commands;
using Catalog.Application.Mappers.CategoryMappers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.CategoryHandler
{
    public class CreateCategoryHandler(
            ICategoryRepository categoryRepository
        ) : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = CategoryMapper.Mapper.Map<Category>(request);
            if (entity is null)
            {
                throw new ApplicationException("not mapped");
            }

            var response = await categoryRepository.CategoryCreate(entity);
            return response;
        }
    }
}
