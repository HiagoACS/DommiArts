using DommiArts.API.DTOs.Product;
namespace DommiArts.API.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}