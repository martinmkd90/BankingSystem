using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class LoanType
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Personal", "Mortgage"
        public double InterestRate { get; set; }
    }
}
