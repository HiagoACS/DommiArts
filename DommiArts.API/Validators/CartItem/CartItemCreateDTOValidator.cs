using FluentValidation;
using DommiArts.API.DTOs.CartItem;

namespace DommiArts.API.Validators.CartItem
{
    public class CartItemCreateDTOValidator : AbstractValidator<CartItemAddDTO>
    {
        public CartItemCreateDTOValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}