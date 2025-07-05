using FluentValidation;
using DommiArts.API.DTOs.Product;
namespace DommiArts.API.Validator.Product
{
       public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateDTOValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            /* Descomentar se necessário uma URL de imagem válida
             RuleFor(p => p.ImageUrl)
                 .NotEmpty().WithMessage("Image URL is required.")
                 .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                 .WithMessage("Image URL must be a valid absolute URL.");
             */

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("Category ID is required.")
                .GreaterThan(0).WithMessage("Category ID must be greater than zero.");
        }
    }
}