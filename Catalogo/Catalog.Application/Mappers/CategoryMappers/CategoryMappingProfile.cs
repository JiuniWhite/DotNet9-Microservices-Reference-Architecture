using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers.CategoryMappers;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, Responses.CategoryResponses.CategoryResponse>().ReverseMap();
        CreateMap<Category, CreateCategoryCommand>().ReverseMap();
        CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
    }
}
