using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomatedInvestmentController : ControllerBase
    {
        private readonly IAutomatedInvestmentService _automatedInvestmentService;

        public AutomatedInvestmentController(IAutomatedInvestmentService automatedInvestmentService)
        {
            _automatedInvestmentService = automatedInvestmentService;
        }

        [HttpGet("getRule")]
        public ActionResult<AutomatedInvestmentRule> GetRule(int ruleId)
        {
            var result = _automatedInvestmentService.GetRule(ruleId);
            return Ok(result);
        }

        [HttpPost("createRule")]
        public ActionResult<AutomatedInvestmentRule> CreateRule([FromBody] AutomatedInvestmentRule rule)
        {
            var createdRule = _automatedInvestmentService.CreateRule(rule);
            return Ok(createdRule);
        }

        [HttpPut("updateRule")]
        public ActionResult<AutomatedInvestmentRule> UpdateRule([FromBody] AutomatedInvestmentRule rule)
        {
            var updatedRule = _automatedInvestmentService.UpdateRule(rule);
            return Ok(updatedRule);
        }

        [HttpDelete("deleteRule/{ruleId}")]
        public ActionResult DeleteRule(int ruleId)
        {
            _automatedInvestmentService.DeleteRule(ruleId);
            return Ok();
        }

        [HttpGet("getUserRules/{userId}")]
        public ActionResult<IEnumerable<AutomatedInvestmentRule>> GetUserRules(int userId)
        {
            var rules = _automatedInvestmentService.GetUserRules(userId);
            return Ok(rules);
        }
    }

}
