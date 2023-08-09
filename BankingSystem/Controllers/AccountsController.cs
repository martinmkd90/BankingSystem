using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            var account = _accountService.GetAccount(id);
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost("{id}/deposit")]
        public IActionResult Deposit(int id, [FromBody] double amount)
        {
            var success = _accountService.Deposit(id, amount);
            if (!success)
                return BadRequest("Deposit failed.");

            return Ok(new { Message = "Deposit successful" });
        }

        [HttpPost("{id}/withdraw")]
        public IActionResult Withdraw(int id, [FromBody] double amount)
        {
            var success = _accountService.Withdraw(id, amount);
            if (!success)
                return BadRequest("Withdrawal failed or insufficient funds.");

            return Ok(new { Message = "Withdrawal successful" });
        }
    }

}
