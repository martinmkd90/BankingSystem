using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.Services;
using Banking.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Banking.Services.Models;
using Microsoft.EntityFrameworkCore;
using Banking.Data.Context;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly BankingDbContext _context;
        private readonly IOptions<TokenSettings> _tokenSettings;

        public LoginController(BankingDbContext context, ILoginService loginService, IUserService userService, IOptions<TokenSettings> tokenSettings)
        {
            _loginService = loginService;
            _userService = userService;
            _context = context;
            _tokenSettings = tokenSettings;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            var passwordErrors = PasswordPolicy.ValidatePassword(userDto.Password);
            if (passwordErrors.Any())
            {
                return BadRequest(new { errors = passwordErrors });
            }

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("change-password-with-verification")]
        public IActionResult ChangePasswordWithVerification([FromBody] ChangePasswordWithVerificationDto dto)
        {
            var user = _userService.GetUserById(dto.UserId);
            if (!_userService.VerifyMfaCode(user.Id, dto.VerificationCode))
            {
                return BadRequest("Invalid verification code.");
            }

            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            var user = _loginService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
            {
                if (_loginService.IsAccountLocked(userDto.Username))
                {
                    return BadRequest("Account is locked due to multiple failed login attempts.");
                }
                return BadRequest("Username or password is incorrect.");
            }

            // If multi-factor authentication is enabled, handle that logic here
            if (user.IsMfaEnabled)
            {
                // Generate and send MFA code to the user (via email, SMS, etc.)
                string mfaCode = _userService.GenerateMfaCode();
                _userService.SendMfaCodeToUser(user, mfaCode);

                // Store the generated MFA code in the MfaCodes table
                var mfaCodeEntry = new MfaCode
                {
                    UserId = user.Id,
                    Code = mfaCode,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10) // expires in 10 minutes
                };

                _context.MfaCodes.Add(mfaCodeEntry);
                _context.SaveChanges();

                return Ok(new { RequiresMfaVerification = true, UserId = user.Id });
            }

            // If MFA is not enabled, generate a JWT token
            var token = _userService.GenerateJwtToken(user);

            // Create a session record in the database
            var userSession = new UserSession
            {
                UserId = user.Id,
                Token = token,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenSettings.Value.TokenExpiryMinutes)
            };

            _context.UserSessions.Add(userSession);
            _context.SaveChanges();

            return Ok(new { token });
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
            var token = _userService.GenerateJwtToken(user);
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