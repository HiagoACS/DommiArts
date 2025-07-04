namespace DommiArts.API.DTOs.Product
{
    public class ProductCreateDTO
    {
        public string? Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; } = null!;

        public string? ImageUrl { get; set; } = null!;
        public int StockQuantity { get; set; }
    }
}