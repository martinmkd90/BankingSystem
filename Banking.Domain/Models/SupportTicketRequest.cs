using Banking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class SupportTicketRequest
    {
        [Key]
        public int TicketId { get; set; }
        public int UserId { get; set; } // Foreign Key to User
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; } // e.g., Open, In Progress, Resolved, Closed
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Priority { get; set; } // e.g., Low, Medium, High, Critical
        public bool IsFromSupportTeam { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<SupportTicketResponse> Responses { get; set; }
    }    
}
