using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.DTOs;

namespace Banking.Services.Interfaces
{
    public interface ISupportService
    {
        SupportTicketRequest CreateTicket(SupportTicketRequest ticket);
        void UpdateTicketStatus(int ticketId, TicketStatus status);
        IEnumerable<SupportTicketRequest> GetAllTicketsForUser(int userId);
        SupportTicketRequest GetTicketDetails(int ticketId);
        void AddResponseToTicket(int ticketId, SupportTicketResponse response);
    }
}
