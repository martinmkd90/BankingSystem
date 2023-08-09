using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface ILoanCalculatorService
    {
        LoanCalculationResult CalculateLoanDetails(LoanCalculationRequest request);
        LoanCalculationResult CalculateEarlyRepayment(LoanCalculationRequest request, double earlyRepaymentAmount);
        List<LoanCalculationResult> CompareLoans(List<LoanCalculationRequest> requests);
        bool CheckLoanEligibility(double creditScore, double income);
    }
}
