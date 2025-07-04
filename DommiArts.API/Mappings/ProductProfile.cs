using AutoMapper;
using DommiArts.API.DTOs.Product;
using DommiArts.API.Models;

namespace DommiArts.API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        { 
            CreateMap<Product, ProductDTO>(); // Mapeamento do Get
            CreateMap<ProductCreateDTO, Product>(); // Mapeamento do Post
            CreateMap<ProductUpdateDTO, Product>(); // Mapeamento do Put
        }
    }
}