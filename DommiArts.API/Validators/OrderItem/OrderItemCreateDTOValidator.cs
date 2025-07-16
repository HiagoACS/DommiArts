using FluentValidation;
using DommiArts.API.DTOs.Order;
using DommiArts.API.DTOs.OrderItem;

namespace DommiArts.API.Validators.OrderItem
{
    public class OrderItemCreateDTOValidator : AbstractValidator<OrderItemCreateDTO>
    {
        public OrderItemCreateDTOValidator()
        {
            RuleFor(oi => oi.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}