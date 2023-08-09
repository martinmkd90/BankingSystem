using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class InterestRate
    {
        public int Id { get; set; }
        public string AccountType { get; set; } // e.g., "Savings", "Checking"
        public double Rate { get; set; } // Annual interest rate in percentage
        public DateTime EffectiveFrom { get; set; } // Date from which this rate is effective
    }

}
