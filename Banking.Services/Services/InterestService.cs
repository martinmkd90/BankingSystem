using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class InterestService : IInterestService
    {
        private readonly BankingDbContext _context;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public InterestService(BankingDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public double CalculateInterest(int accountId, DateTime fromDate, DateTime toDate)
        {
            var account = _accountService.GetAccount(accountId);
            var interestRate = _context.InterestRates
                .Where(r => r.AccountType == account.AccountType.Name && r.EffectiveFrom <= toDate)
                .OrderByDescending(r => r.EffectiveFrom)
                .FirstOrDefault();

            if (interestRate == null)
            {
                throw new InvalidOperationException("No applicable interest rate found.");
            }

            var days = (toDate - fromDate).TotalDays;
            var dailyRate = interestRate.Rate / 365 / 100; // Assuming 365 days in a year for simplicity

            return account.Balance * dailyRate * days;
        }

        public void ApplyInterest(int accountId)
        {
            var interest = CalculateInterest(accountId, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
            _accountService.Deposit(accountId, interest);

            // Create a transaction record for the interest application
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = interest,
                Date = DateTime.UtcNow,
                Type = TransactionType.Deposit,
                Description = "Interest Deposit"
            };

            _transactionService.AddTransaction(transaction);
        }

    }

}
