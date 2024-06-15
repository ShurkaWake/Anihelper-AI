using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validators.AppUser
{
    public static class Rules
    {
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$";
        public const string PasswordMessage =
           "'{PropertyName}' should be between 6 and 50 characters and contain at least one" +
           " lowercase letter, uppercase letter, special character and numeric symbol";
    }
}
