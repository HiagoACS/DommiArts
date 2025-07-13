namespace DommiArts.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; } = null;

        public int Quantity { get; set; } = 1;

        public decimal UnitPrice { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    }
}