using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class Investment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InstrumentType { get; set; } // e.g., Stock, Bond, Mutual Fund
        public string InstrumentName { get; set; } // e.g., Apple Inc., US Treasury Bond
        public double AmountInvested { get; set; }
        public DateTime InvestmentDate { get; set; }
        public double CurrentValue { get; set; } // This would be updated based on market values

        // Navigation property
        public List<InvestmentHistory> InvestmentHistories { get; set; }
    }

    public class InvestmentHistory
    {
        public int Id { get; set; }
        public int InvestmentId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; } // or any other relevant fields

        // Navigation properties
        public Investment Investment { get; set; }
    }


}
