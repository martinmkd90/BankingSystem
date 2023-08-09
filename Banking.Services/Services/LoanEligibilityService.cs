using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.DTOs;

namespace Banking.Services.Services
{
    public class LoanEligibilityService : ILoanEligibilityService
    {
        public LoanEligibilityService()
        {
          
        }
        public bool CheckLoanEligibility(LoanEligibilityRequest request)
        {
            double score = 0;

            // Credit Score Contribution
            score += (request.CreditScore / 850) * 40;  // Assuming 850 is the max score, and it contributes 40% to the total score

            // Debt-to-Income Ratio
            double debtToIncomeRatio = request.ExistingDebt / request.Income;
            score += (1 - debtToIncomeRatio) * 20;  // Contributes 20% to the total score

            // Employment Check
            score += request.IsEmployed ? 10 : 0;  // 10% contribution

            // Loan Amount and Term Check
            double loanFactor = (request.RequestedLoanAmount / (request.Income * request.RequestedLoanTermInYears));
            score += (1 - loanFactor) * 10;  // 10% contribution

            // Positive History with the Bank
            score += request.HasPositiveBankHistory ? 5 : 0;  // 5% contribution

            // Age Factor
            score += (request.Age >= 30 && request.Age <= 50) ? 5 : 0;  // Age between 30 and 50 contributes 5%

            // Property Ownership
            score += request.OwnsProperty ? 5 : 0;  // 5% contribution

            // Other Financial Commitments
            double financialCommitmentFactor = request.OtherFinancialCommitments / request.Income;
            score -= financialCommitmentFactor * 5;  // Deduct up to 5% based on other commitments

            // If the score is above a certain threshold, say 70, the loan is approved
            return score >= 70;
        }
    }
}
