using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Banking.Services.Interfaces;
using Banking.Services.DTOs;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Convert.ToInt32(User.Identity);
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest model)
        {
            var userId = Convert.ToInt32(User.Identity);
            var result = await _userService.UpdateUserProfileAsync(userId, model);

            if (result == null)
                return NotFound();            

            return Ok("Profile updated successfully");
        }

    }
}
