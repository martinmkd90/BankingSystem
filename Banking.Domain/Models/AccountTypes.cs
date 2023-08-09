using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class AccountTypes
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Savings", "Checking"
        public string Description { get; set; }
        public bool SupportsOverdraft { get; set; }
        public double OverdraftLimit { get; set; }
        public double InterestRate { get; set; } // For savings accounts
    }
}

