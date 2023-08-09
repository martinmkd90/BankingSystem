using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/aggregator")]
    public class AccountAggregatorController : ControllerBase
    {
        private readonly IAccountAggregatorService _service;

        public AccountAggregatorController(IAccountAggregatorService service)
        {
            _service = service;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAggregatedAccounts()
        {
            var userId = GetUserIdFromClaims();  // Assuming you have a method to get the user ID from the JWT claims.
            var accounts = await _service.GetAllExternalAccountsForUser(userId);
            return Ok(accounts);
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkAccount(AccountLinkingRequest request)
        {
            var userId = GetUserIdFromClaims();
            await _service.LinkExternalAccount(userId, request);
            return Ok();
        }

        [HttpPost("refresh/{externalAccountId}")]
        public async Task<IActionResult> RefreshAccountData(int externalAccountId)
        {
            var userId = GetUserIdFromClaims();
            await _service.RefreshAccountData(userId, externalAccountId);
            return Ok();
        }

        private int GetUserIdFromClaims()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }

}
