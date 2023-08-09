using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialInsightController : ControllerBase
    {
        private readonly IFinancialInsightService _financialInsightService;

        public FinancialInsightController(FinancialInsightService financialInsightService)
        {
            _financialInsightService = financialInsightService;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<FinancialInsight>> GetInsightsForUser(int userId)
        {
            _financialInsightService.GenerateInsightsForUser(userId);
            var insights = _financialInsightService.GetInsightsForUser(userId);
            return Ok(insights);
        }
    }

}
