using Banking.Domain.Enums;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("apply")]
        public IActionResult ApplyForLoan([FromBody] LoanApplicationDto applicationDto)
        {
            try
            {
                var loan = _loanService.ApplyForLoan(applicationDto);
                return Ok(new { Message = "Loan application submitted.", LoanId = loan.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("approve/{loanId}")]
        public IActionResult ApproveLoan(int loanId)
        {
            var loan = _loanService.ApproveLoan(loanId);

            if (loan.Status != LoanStatus.Pending)
                throw new InvalidOperationException("Loan is not in a valid state for this action.");
            if (loan == null)
                return NotFound();

            return Ok(loan);
        }

        [HttpPost("reject/{loanId}")]
        public IActionResult RejectLoan(int loanId)
        {
            var loan = _loanService.RejectLoan(loanId);
            if (loan == null)
                return NotFound();

            return Ok(loan);
        }

        [HttpPost("repay/{loanId}")]
        public IActionResult MakeRepayment([FromBody] LoanRepaymentDto repaymentDto)
        {
            var success = _loanService.MakeRepayment(repaymentDto);
            if (!success)
                return BadRequest("Repayment failed.");

            return Ok("Repayment successful.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserLoans(int userId)
        {
            return Ok( await _loanService.GetUserLoans(userId));
        }
    }
}
