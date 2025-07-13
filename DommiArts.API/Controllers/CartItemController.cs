using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DommiArts.API.Data;
using DommiArts.API.Models;
using DommiArts.API.DTOs.CartItem;

namespace DommiArts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly DommiArtsDbContext _context;
        private readonly IMapper _mapper;

        public CartItemsController(DommiArtsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/cartitems/cart/{cartId}
        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetItemsByCart(int cartId)
        {
            var items = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<CartItemDTO>>(items));
        }

        // POST: api/cartitems
        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> Create([FromBody] CartItemCreateDTO dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            var cartExists = await _context.Carts.AnyAsync(c => c.Id == dto.CartId);
            if (!cartExists)
                return NotFound("Cart not found.");

            var cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = product.Price, // <-- AQUI o preço é corretamente atribuído
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            var result = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItem.Id); // Obtém o item recém-criado com o produto incluído

            return CreatedAtAction(nameof(GetItemsByCart), new { cartId = dto.CartId }, _mapper.Map<CartItemDTO>(result));
        }

        // POST: api/cartitems/add
        [HttpPost("add")]
        public async Task<IActionResult> AddOrUpdateItem([FromQuery] int cartId, [FromBody] CartItemAddDTO dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/cartitems/updateQuantity/{cartItemId}
        [HttpPut("updateQuantity/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] CartItemUpdateDTO dto)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item == null)
                return NotFound("Cart item not found.");

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Quantity updated successfully." });
        }

        // DELETE: api/cartitems
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int cartId, [FromQuery] int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (item == null)
                return NotFound("Cart item not found for the given cart and product.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
