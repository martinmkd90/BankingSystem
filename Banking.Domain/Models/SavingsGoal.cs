using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string GoalName { get; set; }  // e.g., Vacation to Hawaii
        public double TargetAmount { get; set; }
        public double CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
    }

}
