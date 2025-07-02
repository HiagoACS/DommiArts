namespace DommiArts.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation property to the collection of products in this category
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}