using DommiArts.API.DTOs.Product;
namespace DommiArts.API.DTOs.Category
{
    public class CategoryUpdateDTO
    {
        public string Name { get; set; } = null!;
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}