namespace Banking.API.Controllers
{
    using Microsoft.AspNetCore.Mvc; 
    using global::Banking.Domain.Models;
    using global::Banking.Services.Interfaces;

    namespace Banking.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class LoanCalculatorController : ControllerBase
        {
            private readonly ILoanCalculatorService _loanCalculatorService;

            public LoanCalculatorController(ILoanCalculatorService loanCalculatorService)
            {
                _loanCalculatorService = loanCalculatorService;
            }

            [HttpPost("calculate")]
            public ActionResult<LoanCalculationResult> CalculateLoan([FromBody] LoanCalculationRequest request)
            {
                if (request == null)
                {
                    return BadRequest("Invalid request.");
                }

                var result = _loanCalculatorService.CalculateLoanDetails(request);
                return Ok(result);
            }
        }
    }

}
