using Microsoft.AspNetCore.Mvc;
using Banking.Data.Context;
using System.Linq;
using System.Threading.Tasks;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.DTOs;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;

        public InvestmentController(IInvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        [HttpPost("invest")]
        public ActionResult<Investment> Invest([FromBody] InvestmentRequest request)
        {
            var investment = _investmentService.Invest(request.UserId, request.InstrumentType, request.InstrumentName, request.Amount);
            return Ok(investment);
        }

        [HttpPut("updateValue/{investmentId}")]
        public ActionResult UpdateInvestmentValue(int investmentId, [FromBody] double newValue)
        {
            _investmentService.UpdateInvestmentValue(investmentId, newValue);
            return Ok();
        }

        [HttpGet("userInvestments/{userId}")]
        public ActionResult<IEnumerable<Investment>> GetUserInvestments(int userId)
        {
            var investments = _investmentService.GetUserInvestments(userId);
            return Ok(investments);
        }

        [HttpGet("{investmentId}/history")]
        public ActionResult<IEnumerable<InvestmentHistory>> GetInvestmentHistory(int investmentId)
        {
            var historyData = _investmentService.GetInvestmentHistory(investmentId);
            if (historyData == null || !historyData.Any())
            {
                return NotFound($"No history data found for investment with ID {investmentId}.");
            }
            return Ok(historyData);
        }

    }

}
