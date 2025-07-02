using Microsoft.AspNetCore.Mvc;
using DommiArts.API.Data;
using DommiArts.API.Models;

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
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
    }
}