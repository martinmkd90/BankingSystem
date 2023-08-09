using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDashboard(int userId)
        {
            var dashboard = await _dashboardService.GetUserDashboard(userId);
            if (dashboard == null)
            {
                return NotFound();
            }
            return Ok(dashboard);
        }

    }

}
