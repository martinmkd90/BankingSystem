using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Models
{
    public class SupportTicketResponse
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; } // Foreign Key to SupportTicketRequest
        public int UserId { get; set; } // Foreign Key to User - this can be the user or a support agent
        public string Message { get; set; }
        public DateTime RespondedAt { get; set; }
        public bool IsFromSupportTeam { get; set; }

        // Navigation properties
        public SupportTicketRequest Ticket { get; set; }
        public User User { get; set; }
    }
}
