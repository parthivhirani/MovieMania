using DemoPractical.ViewModels;
using FluentValidation;

namespace DemoPractical.Validations
{
    public class ProfileValidator: AbstractValidator<UpdatePasswordModel>
    {
        public ProfileValidator()
        {
            RuleFor(p => p.CurrentPassword).NotEmpty();
            RuleFor(p => p.NewPassword).NotEmpty();
            RuleFor(p => p.ConfirmNewPassword).NotEmpty().Equal(u => u.NewPassword).WithMessage("Password and confirm password doesn't matched!");
        }
    }
}
