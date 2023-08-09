using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        public double CreditScore { get; set; }  // Ranges typically from 300-850
        public double AnnualIncome { get; set; }  // User's annual income
        public double ExistingLoanAmount { get; set; }  // Total amount of existing loans
        public string EmploymentStatus { get; set; }  // e.g., Employed, Self-Employed, Unemployed
        public int EmploymentDurationInMonths { get; set; }  // Duration of current employment
        public string EmploymentSector { get; set; }  // e.g., IT, Healthcare, Finance
        public bool HasPreviousLoanDefaults { get; set; }  // If the user has defaulted on loans before
    }

}
