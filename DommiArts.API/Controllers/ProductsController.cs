using Microsoft.AspNetCore.Mvc;
using DommiArts.API.Data;
using DommiArts.API.DTOs.Product;
using DommiArts.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DommiArts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        public ProductsController(DommiArtsDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts() //IEnumerable<ProductDTO> para retornar uma lista de produtos
        {
            var products = await _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    StockQuantity = p.StockQuantity
                }).ToListAsync();

            return Ok(products); // Retorna a lista de produtos com suas categorias
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductCreateDTO dto) // usando [FromBody] para receber o objeto JSON
        {
            Product product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                StockQuantity = dto.StockQuantity
            };

            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            // Criado DTO de resposta para retornar o produto criado
            ProductDTO createdProduct = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, createdProduct);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductsById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id).Select(ProductDTO => new ProductDTO
                {
                    Name = ProductDTO.Name,
                    Price = ProductDTO.Price,
                    Description = ProductDTO.Description,
                    ImageUrl = ProductDTO.ImageUrl,
                    StockQuantity = ProductDTO.StockQuantity
                }).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO dto)
        {

            if (!ModelState.IsValid) // Verifica se o modelo é válido
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Atualiza as propriedades do produto com os dados do DTO
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.StockQuantity = dto.StockQuantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}