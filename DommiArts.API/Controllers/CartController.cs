using AutoMapper;
using DommiArts.API.Data;
using DommiArts.API.DTOs.Cart;
using DommiArts.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DommiArts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        private readonly IMapper _mapper;

        public CartController(DommiArtsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/cart
        [HttpPost]
        public async Task<ActionResult<CartDTO>> CreateCart()
        {
            var cart = new Cart();
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cartDTO);
        }

        // GET: api/cart/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDTO>> GetCartById(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
                return NotFound("Cart not found.");

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return Ok(cartDTO);
        }

        // GET: api/cart/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDTO>> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart for user not found.");

            var cartDTO = _mapper.Map<CartDTO>(cart);
            return Ok(cartDTO);
        }

        // PUT: api/cart/{cartId}/assign-user/{userId}
        [HttpPut("{cartId}/assign-user/{userId}")]
        public async Task<IActionResult> AssignCartToUser(int cartId, int userId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
                return NotFound("Cart not found.");

            cart.UserId = userId;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
