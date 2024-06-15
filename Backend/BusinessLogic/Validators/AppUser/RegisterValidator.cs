using BusinessLogic.ViewModels.AppUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogic.Validators.AppUser.Rules;

namespace BusinessLogic.Validators.AppUser
{
    public class RegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).Matches(PasswordRegex).WithMessage(PasswordMessage);
            RuleFor(x => x.RepeatPassword).Equal(x => x.Password);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(32);
        }
    }
}
