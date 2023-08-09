using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class SupportMessage
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public bool IsFromBot { get; set; }

        // Navigation properties
        public virtual SupportTicketRequest Ticket { get; set; }
        public virtual User Sender { get; set; }
    }

}
