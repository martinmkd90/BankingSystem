using Banking.Data.Context;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Banking.Services.Models;
using Banking.Services.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly BankingDbContext _context;
        private readonly IOptions<TokenSettings> _tokenSettings;

        public AuthenticationController(IUserService userService, BankingDbContext context, IOptions<TokenSettings> tokenSettings)
        {
            _userService = userService;
            _context = context;
            _tokenSettings = tokenSettings;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto model)
        {
            if (model.RoleId == null)
            {
                var defaultRole = _userService.GetRoleByName("Customer");
                if (defaultRole != null)
                {
                    model.RoleId = defaultRole.Id;
                }
                else
                {
                    return BadRequest("Customer role not found.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _userService.GetUserByUsername(model.Username);
            if (existingUser != null)
            {
                return Conflict(new
                {
                    error = new
                    {
                        code = "USERNAME_EXISTS",
                        message = "The username is already taken. Please choose another one."
                    }
                });
            }

            var existingEmail = _userService.GetUserByEmail(model.Email);
            if (existingEmail != null)
            {
                return Conflict(new
                {
                    error = new
                    {
                        code = "EMAIL_EXISTS",
                        message = "An account with this email already exists."
                    }
                });
            }

            var user = _userService.Register(model);
            if (user == null)
                return BadRequest("Registration failed.");

            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto model)
        {
            var user = _userService.Login(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            // If multi-factor authentication is enabled, handle that logic here
            if (user.IsMfaEnabled)
            {
                // Generate and send MFA code to the user (via email, SMS, etc.)
                string mfaCode = _userService.GenerateMfaCode();
                _userService.SendMfaCodeToUser(user, mfaCode);

                var mfaCodeEntry = new MfaCode
                {
                    UserId = user.Id,
                    Code = mfaCode,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(_tokenSettings.Value.TokenExpiryMinutes)
                };

                _context.MfaCodes.Add(mfaCodeEntry);
                _context.SaveChanges();

                return Ok(new { RequiresMfaVerification = true, UserId = user.Id });
            }

            var jwtToken = _userService.GenerateJwtToken(user, Response);

            return Ok(new { User = user, Token = jwtToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_cookie");
            return Ok(new { message = "Logged out successfully" });
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = _userService.GetUserById(dto.UserId);

            // 1. Password Policy Enforcement
            var passwordErrors = PasswordPolicy.ValidatePassword(dto.NewPassword);
            if (passwordErrors.Any())
            {
                return BadRequest(new { errors = passwordErrors });
            }

            // 2. Password Change Policy
            if (user.PreviousPasswordHashes.Contains(_userService.HashPassword(dto.NewPassword)))
            {
                return BadRequest("You can't reuse one of your last passwords.");
            }

            // 3. Two-Step Verification for Password Changes
            if (!_userService.VerifyMfaCode(user.Id, dto.VerificationCode))
            {
                return BadRequest("Invalid verification code.");
            }

            // 4. Check Passwords Against Known Breaches
            var isCompromised = await _userService.IsPasswordCompromised(dto.NewPassword);
            if (isCompromised)
            {
                return BadRequest("This password has been compromised in a known breach. Please choose a different password.");
            }

            // If all checks pass, proceed with the password change
            user.PasswordHash = _userService.HashPassword(dto.NewPassword);
            user.LastPasswordChangeDate = DateTime.UtcNow;
            user.PreviousPasswordHashes.Add(user.PasswordHash);
            _userService.UpdateUser(user);

            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto request)
        {
            try
            {
                await _userService.RequestPasswordReset(request.Email);
                return Ok(new { Message = "Password reset link has been sent to your email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto model)
        {
            var result = await _userService.ResetPassword(model);
            if (result)
            {
                return Ok(new { Message = "Password reset successfully." });
            }
            return BadRequest(new { Message = "Invalid or expired token." });
        }

        // This is a placeholder and should be replaced with actual MFA verification logic
        [HttpPost("verify-mfa")]
        public IActionResult VerifyMfa([FromBody] MfaVerificationDto mfaVerification)
        {
            bool isVerified = _userService.VerifyMfaCode(mfaVerification.UserId, mfaVerification.Code);
            if (!isVerified) return BadRequest("Invalid MFA code.");

            // Generate and return the JWT token
            var user = _userService.GetUserById(mfaVerification.UserId);
            var token = _userService.GenerateJwtToken(user, Response);
            return Ok(new { token });
        }

        private void SendPasswordResetEmail(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App Name", "yourapp@example.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Password Reset Request";

            message.Body = new TextPart("plain")
            {
                Text = $"Please click on the following link to reset your password: {resetLink}"
            };

            using var client = new SmtpClient();
            client.Connect("smtp.example.com", 587, false); // SMTP server details
            client.Authenticate("your_email@example.com", "your_password");
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
