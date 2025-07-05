using FluentValidation; // Adicionando FluentValidation para validação de DTOs
using DommiArts.API.DTOs.Product; // 
namespace DommiArts.API.Validators.Product
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(2, 100).WithMessage("Product name must be between 2 and 100 characters.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .Length(10, 500).WithMessage("Product description must be between 10 and 500 characters.");
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");
            RuleFor(p => p.CategoryId)
                .NotNull().WithMessage("Category ID is required.");
            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
        }
    }
}