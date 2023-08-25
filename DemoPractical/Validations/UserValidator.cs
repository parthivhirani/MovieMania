using DemoPractical.ViewModels;
using FluentValidation;

namespace DemoPractical.Validations
{
    public class UserValidator: AbstractValidator<RegisterViewModel>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(u => u.Password).WithMessage("Password doesn't matched!");
        }
    }
}
