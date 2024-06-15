using BusinessLogic.ViewModels.AppUser;
using FluentValidation;

namespace BusinessLogic.Validators.AppUser
{
    public class LoginValidator : AbstractValidator<UserLoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}