using Banking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class UserRiskProfile
    {
        [Key]
        public int UserId { get; set; }
        public RiskType RiskTolerance { get; set; }
        public InvestmentGoal Goal { get; set; }
        public double AnnualIncome { get; set; }
        public int Age { get; set; }
        public double ExistingInvestmentsValue { get; set; }
    }

}
