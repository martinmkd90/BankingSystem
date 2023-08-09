using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Banking.Services.Services
{
    public class LoanCalculatorService : ILoanCalculatorService
    {
        private readonly ILogger<LoanCalculatorService> _logger;
        private readonly BankingDbContext _context;
        public LoanCalculatorService(BankingDbContext context, ILogger<LoanCalculatorService> logger)
        {
           _context = context;
           _logger = logger;
        }
        public LoanCalculationResult CalculateLoanDetails(LoanCalculationRequest request)
        {
            try
            {
                // Fetch user profile
                var userProfile = _context.UserProfiles.FirstOrDefault(u => u.UserId == request.UserId);
                if (userProfile == null)
                {
                    throw new ArgumentException("User profile not found.");
                }

                // Check loan eligibility based on user profile
                if (!IsUserEligibleForLoan(userProfile))
                {
                    throw new ApplicationException("User is not eligible for a loan based on their profile.");
                }

                // Validate request data
                if (request.LoanTermInYears <= 0 || request.PrincipalAmount <= 0 || request.AnnualInterestRate <= 0)
                {
                    throw new ArgumentException("Invalid loan term, principal amount, or interest rate.");
                }

                var monthlyInterestRate = request.AnnualInterestRate / 12 / 100;  // Convert annual rate to monthly and percentage to decimal
                var numberOfPayments = request.LoanTermInYears * 12;  // Total number of monthly payments

                // Calculate monthly payment using the formula
                var monthlyPayment = request.PrincipalAmount * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, numberOfPayments)) / (Math.Pow(1 + monthlyInterestRate, numberOfPayments) - 1);

                var amortizationSchedule = GenerateAmortizationSchedule(request.PrincipalAmount, monthlyInterestRate, monthlyPayment, numberOfPayments);

                return new LoanCalculationResult
                {
                    MonthlyPayment = monthlyPayment,
                    TotalPayment = monthlyPayment * numberOfPayments,
                    TotalInterest = (monthlyPayment * numberOfPayments) - request.PrincipalAmount,
                    AmortizationSchedule = amortizationSchedule
                };
            }
            catch (DivideByZeroException)
            {
                // Handle division by zero error
                throw new ApplicationException("Error in loan calculation due to invalid input values.");
            }
            catch (Exception ex)
            {
                // Generic error handler
                // Log the exception
                _logger.LogError(ex, "An error occurred during loan calculation.");  // Assuming you have a logger instance named _logger
                throw new ApplicationException("An error occurred during loan calculation.", ex);
            }
        }

        private static bool IsUserEligibleForLoan(UserProfile userProfile)
        {
            // Example eligibility criteria
            return userProfile.CreditScore > 650 && userProfile.AnnualIncome > 50000 && !userProfile.HasPreviousLoanDefaults;
        }


        private static List<AmortizationEntry> GenerateAmortizationSchedule(double principal, double monthlyInterestRate, double monthlyPayment, int numberOfPayments)
        {
            var schedule = new List<AmortizationEntry>();
            var remainingBalance = principal;

            for (int month = 1; month <= numberOfPayments; month++)
            {
                var interestPaid = remainingBalance * monthlyInterestRate;
                var principalPaid = monthlyPayment - interestPaid;
                remainingBalance -= principalPaid;

                schedule.Add(new AmortizationEntry
                {
                    MonthNumber = month,
                    PrincipalPaid = principalPaid,
                    InterestPaid = interestPaid,
                    RemainingBalance = remainingBalance
                });
            }

            return schedule;
        }

        public LoanCalculationResult CalculateEarlyRepayment(LoanCalculationRequest request, double earlyRepaymentAmount)
        {
            // Deduct the early repayment amount from the principal
            request.PrincipalAmount -= earlyRepaymentAmount;

            // Recalculate the loan details
            return CalculateLoanDetails(request);
        }

        public List<LoanCalculationResult> CompareLoans(List<LoanCalculationRequest> requests)
        {
            return requests.Select(r => CalculateLoanDetails(r)).ToList();
        }

        public bool CheckLoanEligibility(double creditScore, double income)
        {
            // Example logic: Users with a credit score above 650 and income above $50,000 are eligible
            return creditScore > 650 && income > 50000;
        }
    }

}
