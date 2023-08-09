using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;

        public InterestController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        [HttpGet("calculate/{accountId}")]
        public ActionResult<double> CalculateInterest(int accountId, DateTime fromDate, DateTime toDate)
        {
            var interest = _interestService.CalculateInterest(accountId, fromDate, toDate);
            return Ok(interest);
        }

        [HttpPost("apply/{accountId}")]
        public IActionResult ApplyInterest(int accountId)
        {
            _interestService.ApplyInterest(accountId);
            return Ok();
        }
    }

}
