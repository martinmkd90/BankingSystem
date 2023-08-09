using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class CustomerSupportService : ICustomerSupportService
    {
        private readonly BankingDbContext _context;

        public CustomerSupportService(BankingDbContext context)
        {
            _context = context;
        }

        public SupportTicketRequest CreateTicket(int userId, string subject, string description)
        {
            var ticket = new SupportTicketRequest
            {
                UserId = userId,
                Subject = subject,
                Description = description
            };

            _context.SupportTicketRequests.Add(ticket);
            _context.SaveChanges();

            return ticket;
        }

        public SupportMessage AddMessageToTicket(int ticketId, string content, bool isFromBot)
        {
            var message = new SupportMessage
            {
                TicketId = ticketId,
                Content = content,
                IsFromBot = isFromBot
            };

            _context.SupportMessages.Add(message);
            _context.SaveChanges();

            return message;
        }

        public SupportTicketRequest GetTicketById(int ticketId)
        {
            return _context.SupportTicketRequests
                .Include(t => t.Responses).AsNoTracking()
                .SingleOrDefault(t => t.TicketId == ticketId);
        }

        public IEnumerable<SupportTicketRequest> GetAllTicketsForUser(int userId)
        {
            return _context.SupportTicketRequests
                .Where(t => t.UserId == userId)
                .Include(t => t.Responses).AsNoTracking()
                .ToList();
        }       

        public void AddResponse(int ticketId, string message, bool isFromSupport)
        {
            var response = new SupportTicketResponse
            {
                Id = ticketId,
                Message = message,
                RespondedAt = DateTime.UtcNow,
                IsFromSupportTeam = isFromSupport
            };

            _context.SupportTicketResponses.Add(response);
            _context.SaveChanges();
        }

        public void UpdateTicketStatus(int ticketId, TicketStatus status)
        {
            var ticket = _context.SupportTicketRequests.Find(ticketId);
            if (ticket != null)
            {
                ticket.Status = status;
                _context.SaveChanges();
            }
        }

        public void ResolveTicket(int ticketId)
        {
            UpdateTicketStatus(ticketId, TicketStatus.Resolved);
        }
    }

}
