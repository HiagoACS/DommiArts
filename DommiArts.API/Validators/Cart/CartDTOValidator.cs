using FluentValidation;
using DommiArts.API.DTOs.Cart;
namespace DommiArts.API.Validators.Cart
{
    public class CartDTOValidator : AbstractValidator<CartDTO>
    {
        public CartDTOValidator()
        {
            RuleFor(cart => cart.Id).GreaterThan(0).WithMessage("Cart ID must be greater than 0.");
            RuleFor(cart => cart.Items)
                .NotNull().WithMessage("Cart items cannot be null.");
        }
    }
}