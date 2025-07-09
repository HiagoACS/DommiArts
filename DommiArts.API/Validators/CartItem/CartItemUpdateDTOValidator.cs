using FluentValidation;
using DommiArts.API.DTOs.CartItem;

namespace DommiArts.API.Validators.CartItem
{
    public class CartItemUpdateDTOValidator : AbstractValidator<CartItemUpdateDTO>
    {
        public CartItemUpdateDTOValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}