using Banking.Domain.Enums;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("request")]
        public IActionResult RequestCard(int userId, CardType cardType)
        {
            var card = _cardService.RequestCard(userId, cardType);
            return Ok(card);
        }

        [HttpPost("activate/{cardId}")]
        public IActionResult ActivateCard(int cardId)
        {
            var card = _cardService.ActivateCard(cardId);
            if (card == null)
                return NotFound();

            return Ok(card);
        }

        [HttpPost("block/{cardId}")]
        public IActionResult BlockCard(int cardId)
        {
            var card = _cardService.BlockCard(cardId);
            if (card == null)
                return NotFound();

            return Ok(card);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserCards(int userId)
        {
            return Ok(_cardService.GetUserCards(userId));
        }
    }

}
