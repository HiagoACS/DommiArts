using AutoMapper;
using DommiArts.API.DTOs.CartItem;
using DommiArts.API.Models;
namespace DommiArts.API.Mappings
{
	public class CartItemProfile : Profile
	{
		public CartItemProfile()
		{
			CreateMap<CartItem, CartItemDTO>(); // Mapeamento do Get
			CreateMap<CartItemCreateDTO, CartItem>(); // Mapeamento do Post
		}
	}
}