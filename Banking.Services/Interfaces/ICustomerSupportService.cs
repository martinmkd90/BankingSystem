using Banking.Domain.Enums;
using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface ICustomerSupportService
    {
        SupportTicketRequest CreateTicket(int userId, string subject, string Description);
        SupportMessage AddMessageToTicket(int ticketId, string content, bool isFromBot);
        SupportTicketRequest GetTicketById(int ticketId);
        IEnumerable<SupportTicketRequest> GetAllTicketsForUser(int userId);
        void UpdateTicketStatus(int ticketId, TicketStatus status);
        void ResolveTicket(int ticketId);
        void AddResponse(int ticketId, string message, bool isFromSupport);
    }
}
