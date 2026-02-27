using MediatR;

namespace Catalog.Application.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
