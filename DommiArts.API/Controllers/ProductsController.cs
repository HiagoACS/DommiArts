using Microsoft.AspNetCore.Mvc;
using DommiArts.API.Data;
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
        public async Task<ActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category) // Incluindo a Categoria relacionada 
                .ToListAsync();

            return Ok(products); // Retorna a lista de produtos com suas categorias
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product) // usando [FromBody] para receber o objeto JSON
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) // Incluindo a Categoria relacionada
                .FirstOrDefaultAsync(p => p.Id == id);

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
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch.");
            }
            _context.Entry(product).State = EntityState.Modified;
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