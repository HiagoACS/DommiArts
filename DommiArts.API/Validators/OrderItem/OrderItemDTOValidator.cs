using FluentValidation;
using DommiArts.API.DTOs.Order;
using DommiArts.API.DTOs.OrderItem;

namespace DommiArts.API.Validators.OrderItem
{
    public class OrderItemDTOValidator : AbstractValidator<OrderItemDTO>
    {
        public OrderItemDTOValidator()
        {
            RuleFor(oi => oi.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(oi => oi.UnitPrice)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}