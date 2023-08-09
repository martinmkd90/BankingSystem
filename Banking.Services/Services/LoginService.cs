using Banking.Domain.Models;
using Banking.Data.Context;
using System.Linq;
using Banking.Services.Interfaces;

namespace Banking.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly BankingDbContext _context;
        private readonly IUserService _userService;
        private const int MaxFailedAttempts = 5; // You can adjust this value

        public LoginService(BankingDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            // Verify password
            if (user == null || !_userService.VerifyPassword(password, user.PasswordHash))
            {
                IncrementFailedLoginAttempt(username);
                return null;
            }

            if (IsAccountLocked(username))
            {
                // Handle account lockout logic, e.g., notify the user
                return null;
            }

            ResetFailedLoginAttempt(username);
            return user;
        }

        public bool IsAccountLocked(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return false;

            return user.FailedLoginAttempts >= MaxFailedAttempts;
        }        

        public void IncrementFailedLoginAttempt(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return;

            user.FailedLoginAttempts++;
            _context.SaveChanges();
        }

        public void ResetFailedLoginAttempt(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return;

            user.FailedLoginAttempts = 0;
            _context.SaveChanges();
        }

        public void RecordFailedLoginAttempt(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.FailedLoginAttempts += 1;
                if (user.FailedLoginAttempts >= 5) // Assuming 5 failed attempts will lock the account
                {
                    user.IsLocked = true;
                }
                _context.SaveChanges();
            }
        }        
    }
}
