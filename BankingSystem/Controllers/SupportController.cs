using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        [HttpPost("create")]
        public ActionResult<SupportTicketRequest> CreateTicket([FromBody] SupportTicketRequest ticket)
        {
            var createdTicket = _supportService.CreateTicket(ticket);
            return Ok(createdTicket);
        }

        [HttpPut("updateStatus/{ticketId}")]
        public IActionResult UpdateTicketStatus(int ticketId, [FromBody] TicketStatus status)
        {
            _supportService.UpdateTicketStatus(ticketId, status);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<SupportTicketRequest>> GetAllTicketsForUser(int userId)
        {
            var tickets = _supportService.GetAllTicketsForUser(userId);
            return Ok(tickets);
        }

        [HttpGet("{ticketId}")]
        public ActionResult<SupportTicketRequest> GetTicketDetails(int ticketId)
        {
            var ticket = _supportService.GetTicketDetails(ticketId);
            return Ok(ticket);
        }

        [HttpPost("response/{ticketId}")]
        public IActionResult AddResponseToTicket(int ticketId, [FromBody] SupportTicketResponse response)
        {
            _supportService.AddResponseToTicket(ticketId, response);
            return Ok();
        }
    }

}
