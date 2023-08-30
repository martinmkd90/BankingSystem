using Banking.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/accounts/{accountId}/transactions")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions(int page = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching all transactions");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))            
                return Unauthorized();
            

            if (!int.TryParse(userIdClaim, out int userId))            
                return BadRequest("Invalid user identifier");
            

            var response = await _transactionService.GetTransactionsForUser(userId, page, pageSize);
            _logger.LogInformation("Fetched all transactions successfully");
            return Ok(response);
        }

        [HttpGet("test-error")]
        [Route("/api/transactions/test-error", Name = "TestError")]
        [AllowAnonymous]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception for development environment.");
        }

    }
}
