
namespace Banking.Domain.Models
{
    public class LoanEligibilityRequest
    {
        public double CreditScore { get; set; }
        public double Income { get; set; }
        public double ExistingDebt { get; set; }
        public bool IsEmployed { get; set; }
        public double RequestedLoanAmount { get; set; }
        public int RequestedLoanTermInYears { get; set; }
        public bool HasPositiveBankHistory { get; set; }
        public int Age { get; set; }
        public bool OwnsProperty { get; set; }
        public double OtherFinancialCommitments { get; set; }
    }
}
