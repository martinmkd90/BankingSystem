using Banking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class ScheduledPayment
    {
        public int Id { get; set; }
        public double Amount { get; set; }  // The amount to be paid
        public string Recipient { get; set; }  // The recipient of the payment (could be another account or an external entity)
        public DateTime NextPaymentDate { get; set; }  // The date on which the next payment should be made
        public int FrequencyInDays { get; set; }  // Frequency of the payment in days (e.g., 30 for monthly)

        // Navigation properties
        public Account Account { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public RecurrenceType Recurrence { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public FailReason FailReason { get; set; } = FailReason.None;
    }
}
