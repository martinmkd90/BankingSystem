using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class SupportService : ISupportService
    {
        private readonly BankingDbContext _context;
        private readonly INotificationService _notificationService;

        public SupportService(BankingDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public SupportTicketRequest CreateTicket(SupportTicketRequest ticket)
        {
            ticket.DateCreated = DateTime.UtcNow;
            ticket.DateUpdated = DateTime.UtcNow;
            _context.SupportTicketRequests.Add(ticket);
            _context.SaveChanges();

            // Notify support agents about the new ticket
            _notificationService.SendNotificationToSupport($"New support ticket from {ticket.UserId}: {ticket.Subject}");

            return ticket;
        }

        public void UpdateTicketStatus(int ticketId, TicketStatus status)
        {
            var ticket = _context.SupportTicketRequests.Find(ticketId);
            if (ticket != null)
            {
                ticket.Status = status;
                ticket.DateUpdated = DateTime.UtcNow;
                _context.SaveChanges();

                // Notify the user about the status change
                _notificationService.SendNotification(ticket.UserId, $"Your support ticket {ticketId} status has been updated to {status}.");
            }
        }

        public IEnumerable<SupportTicketRequest> GetAllTicketsForUser(int userId)
        {
            return _context.SupportTicketRequests.Where(t => t.UserId == userId).ToList();
        }

        public SupportTicketRequest GetTicketDetails(int ticketId)
        {
            return _context.SupportTicketRequests.Include(t => t.Responses).SingleOrDefault(t => t.TicketId == ticketId);
        }

        public void AddResponseToTicket(int ticketId, SupportTicketResponse response)
        {
            var ticket = _context.SupportTicketRequests.Find(ticketId);
            if (ticket != null)
            {
                response.RespondedAt = DateTime.UtcNow;
                _context.SupportTicketResponses.Add(response);
                _context.SaveChanges();

                // Notify the user about the new response
                _notificationService.SendNotification(ticket.UserId, $"Your support ticket {ticketId} has a new response.");
            }
        }
    }
}
