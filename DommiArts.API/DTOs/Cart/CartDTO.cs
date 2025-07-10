using DommiArts.API.DTOs.CartItem;
namespace DommiArts.API.DTOs.Cart
{
    public class CartDTO
    {
        public int Id { get; set; }
        public ICollection<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }
}