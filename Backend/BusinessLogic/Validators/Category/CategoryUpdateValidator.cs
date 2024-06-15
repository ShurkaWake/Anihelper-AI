using BusinessLogic.ViewModels.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators.Category
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateModel>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }
    }
}
