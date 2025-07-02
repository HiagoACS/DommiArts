
namespace DommiArts.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = null;
        public string? Category { get; set; } = null;
        public int StockQuantity { get; set; }

        // Navigation property to the Category entity
        public Category ProductCategory { get; set; } = null!;
    }
}