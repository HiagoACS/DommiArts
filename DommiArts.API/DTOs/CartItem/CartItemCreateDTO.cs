
namespace DommiArts.API.DTOs.CartItem
{
    public class CartItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}