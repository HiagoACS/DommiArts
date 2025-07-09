using Microsoft.AspNetCore.Mvc;
using DommiArts.API.Data;
using DommiArts.API.Models;
using Microsoft.EntityFrameworkCore;
using DommiArts.API.DTOs.Category;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace DommiArts.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        private readonly IMapper _mapper;
        public CategoriesController(DommiArtsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories() //IEnumerable<CategoryDTO> para retornar uma lista de categorias
        {
            var categories = await _context.Categories
                .Include(c => c.Products) // Inclui os produtos relacionados à categoria
                .ToListAsync();

            var categoriesDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories); // Mapeia a lista de categorias para DTOs

            return Ok(categoriesDTOs); 
        }

        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryCreateDTO dto)
        {
            Category category = _mapper.Map<Category>(dto); // Mapeia o DTO para o modelo Category
            if (category == null)
            {
                return BadRequest("Category cannot be null.");
            }

            // Criado DTO de resposta para retornar a categoria criada
            CategoryDTO createdCategory = _mapper.Map<CategoryDTO>(category); // Mapeia o modelo Category para CategoryDTO
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, createdCategory);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products) // Inclui os produtos relacionados à categoria
                .FirstOrDefaultAsync(c => c.Id == id); // Busca a categoria pelo ID

            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category); // Mapeia a categoria para CategoryDTO

            return Ok(categoryDTO);
        }

        // PUT: api/categories/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            _mapper.Map(dto, category); // Mapeia o DTO para o modelo Category


            // Atualizalizando a lista de produtos da categoria (removendo os que não estão mais no DTO e adicionando novos)
            if (dto.Products != null)
            {
                // Criando uma lista de produtos a serem removidos
                var productsToRemove = category.Products
                    .Where(p => !dto.Products.Any(dp => dp.Id == p.Id)) // Verifica se o produto do DTO não está na lista de produtos da categoria
                    .ToList(); // convertendo para lista

                foreach (var product in productsToRemove) // Removendo os produtos que não estão mais no DTO a partir da lista criada
                {
                    category.Products.Remove(product);
                }

                // Atualizando/Adicionando produtos
                foreach (var dtoProduct in dto.Products)
                {
                    var existingProduct = category.Products.FirstOrDefault(p => p.Id == dtoProduct.Id); // Verificando se o produto já existe na categoria pelo ID
                    if (existingProduct != null)
                    {
                        _mapper.Map(dtoProduct, existingProduct); // Atualizando produto existente
                    }
                    else
                    {
                        var newProduct = _mapper.Map<Product>(dtoProduct); //Criando novo produto a partir do DTO
                        category.Products.Add(newProduct); // Adicionando novo produto
                    }
                }
            }
            else
            {
                category.Products.Clear(); // Limpa a lista de produtos se o DTO não contiver produtos
            }


            await _context.SaveChangesAsync();
            return NoContent();

        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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