using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.DTOs
{
    public class InvestmentRequest
    {
        public int UserId { get; set; }
        public string InstrumentType { get; set; } // e.g., Stock, Bond, Mutual Fund
        public string InstrumentName { get; set; } // e.g., Apple Inc., US Treasury Bond
        public double Amount { get; set; }
    }
}
