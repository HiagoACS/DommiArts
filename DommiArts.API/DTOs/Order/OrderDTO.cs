using DommiArts.API.DTOs.OrderItem;
namespace DommiArts.API.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending";
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}