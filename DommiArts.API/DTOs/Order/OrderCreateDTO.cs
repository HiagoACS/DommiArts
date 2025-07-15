using DommiArts.API.DTOs.OrderItem;
namespace DommiArts.API.DTOs.Order
{
    public class OrderCreateDTO
    {
        public int? UserId { get; set; }
        public ICollection<OrderItemCreateDTO> Items { get; set; } = new List<OrderItemCreateDTO>();
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}