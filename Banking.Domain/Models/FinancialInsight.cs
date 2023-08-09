using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class FinancialInsight
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InsightType { get; set; } // e.g., "SpendingPattern", "SavingsTrend"
        public string Description { get; set; } // The insight message to be displayed to the user
        public DateTime DateGenerated { get; set; }
        public bool IsRead { get; set; }
    }

}
