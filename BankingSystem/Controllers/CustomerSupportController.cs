using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSupportController : ControllerBase
    {
        private readonly ICustomerSupportService _supportService;

        public CustomerSupportController(ICustomerSupportService supportService)
        {
            _supportService = supportService;
        }

        [HttpPost("create-ticket")]
        public ActionResult<SupportTicketRequest> CreateTicket([FromBody] SupportTicketRequest request)
        {
            var ticket = _supportService.CreateTicket(request.UserId, request.Subject, request.Description);
            return Ok(ticket);
        }

        [HttpPost("add-response/{ticketId}")]
        public ActionResult AddResponse(int ticketId, [FromBody] SupportTicketRequest request)
        {
            _supportService.AddResponse(ticketId, request.Description, request.IsFromSupportTeam);
            return Ok();
        }       

        [HttpGet("ticket-details/{ticketId}")]
        public ActionResult<SupportTicketRequest> GetTicketDetails(int ticketId)
        {
            var ticket = _supportService.GetTicketById(ticketId);
            return Ok(ticket);
        }

        [HttpPut("update-status/{ticketId}")]
        public ActionResult UpdateTicketStatus(int ticketId, [FromBody] SupportTicketRequest request)
        {
            _supportService.UpdateTicketStatus(ticketId, request.Status);
            return Ok();
        }
    }

}
