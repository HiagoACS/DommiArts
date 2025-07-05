using FluentValidation;
using DommiArts.API.DTOs.User;
namespace DommiArts.API.Validators.User
{
    public class UserAdminCreateDTOValidator : AbstractValidator<UserAdminCreateDTO>
    {
        public UserAdminCreateDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}