using FluentValidation;
using DommiArts.API.DTOs.Order;
using DommiArts.API.Validators.OrderItem;
namespace DommiArts.API.Validators.Order
{
    public class OrderCreateDTOValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDTOValidator()
        {
            RuleFor(o => o.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(o => o.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.")
                .ForEach(item => item.SetValidator(new OrderItemCreateDTOValidator()));
            RuleFor(o => o.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.");
            RuleFor(o => o.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(o => o.CustomerPhone)
                .NotEmpty().WithMessage("Customer phone is required.");
            RuleFor(o => o.DeliveryAddress)
                .NotEmpty().WithMessage("Delivery address is required.");
        }
    }
}