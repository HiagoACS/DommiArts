using FluentValidation;
using DommiArts.API.DTOs.Order;
using DommiArts.API.Validators.OrderItem;

namespace DommiArts.API.Validators.Order
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator()
        {
            RuleFor(o => o.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(o => o.TotalPrice)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
            RuleFor(o => o.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.")
                .ForEach(item => item.SetValidator(new OrderItemDTOValidator()));
        }
    }
}