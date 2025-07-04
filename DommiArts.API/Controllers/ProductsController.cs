using Microsoft.AspNetCore.Mvc;
using DommiArts.API.Data;
using DommiArts.API.DTOs.Product;
using DommiArts.API.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DommiArts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        private readonly IMapper _mapper;
        public ProductsController(DommiArtsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts() //IEnumerable<ProductDTO> para retornar uma lista de produtos
        {
            var products = await _context.Products
                .Include(p => p.Category) // Inclui a categoria relacionada ao produto
                .ToListAsync();

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products); // Mapeia a lista de produtos para DTOs
            return Ok(productDTOs); // Retorna a lista de produtos com suas categorias
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductCreateDTO dto) // usando [FromBody] para receber o objeto JSON
        {
            Product product = _mapper.ConfigurationProvider
                .CreateMapper()
                .Map<Product>(dto); // Mapeia o DTO para o modelo Product

            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            // Criado DTO de resposta para retornar o produto criado
            ProductDTO createdProduct = _mapper.Map<ProductDTO>(product); // Mapeia o modelo Product para ProductDTO

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, createdProduct);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductsById(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id); // Busca o produto pelo ID

            if (product == null)
            {
                return NotFound();
            }

            var productDTO = _mapper.Map<ProductDTO>(product); // Mapeia o produto para ProductDTO

            return Ok(productDTO);
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
            product = _mapper.Map(dto, product); // Mapeia o DTO para o modelo Product existente

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