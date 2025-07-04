namespace DommiArts.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DommiArts.API.Data;
    using DommiArts.API.Models;
    using Microsoft.EntityFrameworkCore;
    using DommiArts.API.DTOs.Category;
    using DommiArts.API.DTOs.Product;
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        public CategoriesController(DommiArtsDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories() //IEnumerable<CategoryDTO> para retornar uma lista de categorias
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,

                    // Seleciona os produtos relacionados a cada categoria
                    Products = c.Products.Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ImageUrl = p.ImageUrl,
                        StockQuantity = p.StockQuantity
                    })
                    .AsEnumerable() // AsEnumerable() para converter IQueryable em IEnumerable
                    .ToList()
                }).ToListAsync();
            return Ok(categories); 
        }
        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            Category category = new Category
            {
                Name = dto.Name
            };
            if (category == null)
            {
                return BadRequest("Category cannot be null.");
            }

            // Criado DTO de resposta para retornar a categoria criada
            CategoryDTO createdCategory = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Products = new List<ProductDTO>() // Inicializa a lista de produtos como vazia
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, createdCategory);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id).Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ImageUrl = p.ImageUrl,
                        StockQuantity = p.StockQuantity
                    })
                    .AsEnumerable() // AsEnumerable() para converter IQueryable em IEnumerable
                    .ToList()
                }).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            // Atualiza os campos da categoria com os dados do DTO
            category.Name = dto.Name;
            // Se houver produtos no DTO, atualiza a lista de produtos
            if (dto.Products != null && dto.Products.Any())
            {
                category.Products = dto.Products.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    StockQuantity = p.StockQuantity
                }).ToList();
            }
            else
            {
                category.Products.Clear(); // Limpa a lista de produtos se não houver produtos no DTO
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}