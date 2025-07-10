using AutoMapper;
using DommiArts.API.DTOs.Cart;
using DommiArts.API.Models;

namespace DommiArts.API.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDTO>();// Mapping for Get
            CreateMap<CartDTO, Cart>();// Mapping for Post
        }
    }
}