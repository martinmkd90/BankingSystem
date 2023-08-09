using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("query")]
        public async Task<ActionResult<string>> HandleQuery([FromBody] ChatbotQueryRequest request)
        {
            if (string.IsNullOrEmpty(request.Query) || string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest("Invalid request parameters.");
            }

            var response = await _chatbotService.HandleUserQuery(request.Query, request.SessionId);
            return Ok(response);
        }
    }
}
