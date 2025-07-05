using FluentValidation;

using DommiArts.API.DTOs.Category;

namespace DommiArts.API.Validators.Category
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
            RuleFor(c => c.Products)
                .NotNull().WithMessage("Products list cannot be null.");
        }
    }
}