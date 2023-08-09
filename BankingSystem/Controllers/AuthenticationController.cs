using Banking.Services.Interfaces;
using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
                return BadRequest("RoleId is required for registration.");

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
                return Unauthorized();

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
