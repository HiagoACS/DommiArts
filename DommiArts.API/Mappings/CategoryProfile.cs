using AutoMapper;
using DommiArts.API.DTOs.Category;
using DommiArts.API.Models;
namespace DommiArts.API.Mappings
{

    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>(); // Mapeamento do Get
            CreateMap<CategoryCreateDTO, Category>(); // Mapeamento do Post
            CreateMap<CategoryUpdateDTO, Category>(); // Mapeamento do Put
        }
    }

}