using FluentValidation;
using DommiArts.API.DTOs.Product;
using DommiArts.API.DTOs.Category;

namespace DommiArts.API.Validators.Category
{
    public class CategoryUpdateDTOValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public CategoryUpdateDTOValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}