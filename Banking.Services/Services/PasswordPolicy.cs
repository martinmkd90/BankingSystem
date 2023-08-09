using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public static class PasswordPolicy
    {
        public static List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password should not be empty.");

            if (password.Length < 8)
                errors.Add("Password should be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("Password should contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                errors.Add("Password should contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                errors.Add("Password should contain at least one number.");

            if (password.Intersect("!@#$%^&*()_+[]{};:<>?|").Count() == 0)
                errors.Add("Password should contain at least one special character.");

            // Add more rules as needed

            return errors;
        }
    }
}
