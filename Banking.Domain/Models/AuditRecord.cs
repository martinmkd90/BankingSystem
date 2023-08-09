using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class AuditRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; } // e.g., "AccountCreated", "TransferMade"
        public string Description { get; set; } // Detailed description of the action
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
