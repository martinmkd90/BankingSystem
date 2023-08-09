using Banking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public virtual Account Account { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
        public string? Location { get; set; }
        public DateTime Timestamp { get; set; }
        public string Category { get; set; }
    }
}
