using Banking.Services.Interfaces;
using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
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

            var jwtToken = _userService.GenerateJwtToken(user, Response);

            return Ok(new { User = user, Token = jwtToken });
        }

    }
}
