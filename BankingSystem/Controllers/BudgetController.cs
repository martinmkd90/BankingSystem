using Banking.Domain.Models;
using Banking.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetService _budgetService;

        public BudgetController(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost("create")]
        public ActionResult<Budget> CreateBudget(int userId, string category, double monthlyLimit)
        {
            var budget = _budgetService.CreateBudget(userId, category, monthlyLimit);
            return Ok(budget);
        }

        [HttpPut("update/{budgetId}")]
        public IActionResult UpdateBudget(int budgetId, double newLimit)
        {
            _budgetService.UpdateBudget(budgetId, newLimit);
            return Ok();
        }

        [HttpDelete("delete/{budgetId}")]
        public IActionResult DeleteBudget(int budgetId)
        {
            _budgetService.DeleteBudget(budgetId);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Budget>> GetBudgetsForUser(int userId)
        {
            var budgets = _budgetService.GetBudgetsForUser(userId);
            return Ok(budgets);
        }

        [HttpPost("trackSpending")]
        public IActionResult TrackSpending(int userId, string category, double amount)
        {
            _budgetService.TrackSpending(userId, category, amount);
            return Ok();
        }
    }

}
