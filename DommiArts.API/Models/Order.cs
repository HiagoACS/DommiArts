
namespace DommiArts.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int? UserId { get; set; } = null;
        public User? User { get; set; } = null;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalAmount { get; set; }

    }
}