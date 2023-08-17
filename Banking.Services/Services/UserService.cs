using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Banking.Services.Services
{
    public class UserService : IUserService
    {
        private readonly BankingDbContext _context;
        private readonly JwtSettings _settings;

        public UserService(BankingDbContext context, IOptions<JwtSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public Role GetRoleByName(string roleName)
        {
            return _context.Roles.SingleOrDefault(r => r.Name == roleName);
        }

        public User Register(UserDto model)
        {
            // Using BCrypt.Net for hashing
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            //var getLastId = _context.Users.Max(u => u.Id);
            var user = new User
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                Email = model.Email,
                RoleId = model.RoleId ?? 1
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User? Login(UserDto model)
        {
            bool isEmail = model.Username != null && model.Username.Contains('@');
            var user = _context.Users.SingleOrDefault(u => isEmail ? u.Email == model.Username : u.Username == model.Username);

            // Check if the user account is locked out
            if (user != null && user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                throw new Exception("Account locked. Try again later.");
            }

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                // Increment failed login attempts
                if (user != null)
                {
                    user.FailedLoginAttempts++;

                    // Lock the account after 5 failed attempts
                    if (user.FailedLoginAttempts >= 5)
                    {
                        user.LockoutEnd = DateTime.Now.AddMinutes(15); // Lock for 15 minutes
                    }

                    _context.SaveChanges();
                }

                return null;
            }

            // Reset failed login attempts on successful login
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            _context.SaveChanges();

            return user;
        }

        public async Task RequestPasswordReset(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                // Optionally, you can throw an exception or return a message indicating the email is not registered.
                return;
            }

            // Generate a unique token
            var token = Guid.NewGuid().ToString();

            // Store the token in the database associated with the user's account
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token is valid for 1 hour
            await _context.SaveChangesAsync();

            // Send an email to the user with the reset link
            var resetLink = $"https://yourwebsite.com/reset-password?token={token}";
            SendPasswordResetEmail(user.Email, resetLink);
        }

        private void SendPasswordResetEmail(string email, string resetLink)
        {
            // Implement logic to send an email. You can use libraries like SendGrid, MailKit, etc.
            // The email should contain the resetLink for the user to click on and reset their password.
        }
        public async Task<bool> ResetPassword(PasswordResetDto model)
        {
            var user = _context.Users.SingleOrDefault(u => u.PasswordResetToken == model.Token);

            if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return false; // Invalid or expired token
            }

            // Update the user's password
            user.PasswordHash = HashPassword(model.NewPassword);
            user.PasswordResetToken = null; // Clear the reset token
            user.PasswordResetTokenExpiry = null; // Clear the token expiry

            await _context.SaveChangesAsync();
            return true;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }


        public bool VerifyMfaCode(int userId, string code)
        {
            var user = _context.Users.Find(userId);
            if (user?.FailedLoginAttempts >= 5 && user.LastMfaAttempt.HasValue && (DateTime.UtcNow - user.LastMfaAttempt.Value).TotalMinutes < 15)
            {
                // User has made 5 failed attempts in the last 15 minutes
                return false;
            }

            var mfaCodeEntry = _context.MfaCodes.FirstOrDefault(m => m.UserId == userId && m.Code == code);
            if (mfaCodeEntry != null && DateTime.UtcNow <= mfaCodeEntry.ExpirationTime)
            {
                user.FailedLoginAttempts = 0; // reset the counter
                _context.SaveChanges();
                return true;
            }
            else
            {
                user.FailedLoginAttempts++;
                user.LastMfaAttempt = DateTime.UtcNow;
                _context.SaveChanges();
                return false;
            }
        }

        public string GenerateMfaCode()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString();
        }

        public List<string> GenerateBackupCodes(int userId, int numberOfCodes = 5)
        {
            var backupCodes = new List<string>();
            for (int i = 0; i < numberOfCodes; i++)
            {
                var code = GenerateMfaCode(); // hypothetical method to generate a random code
                backupCodes.Add(code);

                var backupCodeEntry = new BackupCode
                {
                    UserId = userId,
                    Code = code,
                    Used = false
                };
                _context.BackupCodes.Add(backupCodeEntry);
            }
            _context.SaveChanges();
            return backupCodes;
        }

        public void SendMfaCodeToUser(User user, string mfaCode)
        {
            // TODO: Implement logic to send the MFA code to the user.
            // This could be an email, SMS, or any other method of communication.
            Console.WriteLine($"MFA Code for user {user.Username}: {mfaCode}"); // Just for testing
        }

        public async Task<DateTime?> GetLastLogin(int userId)
        {
            return await _context.Users.Where(u => u.Id == userId).Select(u => u.LastLogin).SingleOrDefaultAsync();
        }
        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public bool IsValidPassword(string password)
        {
            return password.Length >= 8 && // Minimum 8 characters
                   password.Any(char.IsUpper) && // At least one uppercase
                   password.Any(char.IsLower) && // At least one lowercase
                   password.Any(char.IsDigit) && // At least one number
                   password.Any(char.IsPunctuation); // At least one special character
        }

        public string GenerateJwtToken(User user, HttpResponse response)
        {
            var key = Encoding.ASCII.GetBytes(_settings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15), // Token will expire in 15 minutes
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(securityToken);

            // Set the JWT token in an HttpOnly cookie.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            response.Cookies.Append("token", jwtToken, cookieOptions);

            return jwtToken;
        }


        public async Task<bool> IsPasswordCompromised(string password)
        {
            var sha1Password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            var sha1String = BitConverter.ToString(sha1Password).Replace("-", "").Substring(5);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://api.pwnedpasswords.com/range/{sha1String}");
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody.Contains(sha1String);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> UpdateUserProfileAsync(string userId, UpdateProfileRequest model)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) 
                return null;
            
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
