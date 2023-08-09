using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentRecommendationController : ControllerBase
    {
        private readonly IInvestmentRecommendationService _recommendationService;

        public InvestmentRecommendationController(IInvestmentRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("{userId}/recommendations")]
        public ActionResult<IEnumerable<Investment>> GetRecommendations(int userId)
        {
            var recommendations = _recommendationService.GetRecommendations(userId);
            if (recommendations == null || !recommendations.Any())
            {
                return NotFound($"No recommendations found for user with ID {userId}.");
            }
            return Ok(recommendations);
        }
    }

}
