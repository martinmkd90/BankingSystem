using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Category { get; set; }  // e.g., Groceries, Entertainment
        public double MonthlyLimit { get; set; }
        public double AmountSpentThisMonth { get; set; }
    }

}
