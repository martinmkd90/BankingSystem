using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public class FinancialInsightService : IFinancialInsightService
    {
        private readonly BankingDbContext _context;
        private readonly ITransactionService _transactionService;

        public FinancialInsightService(BankingDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        public void GenerateInsightsForUser(int userId)
        {
            var transactions = _transactionService.GetRecentTransactionsForUser(userId, DateTime.UtcNow.AddMonths(-1));

            // Analyze spending habits
            var totalSpent = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);
            if (totalSpent > 5000) // Example threshold
            {
                CreateInsight(userId, "Spending", "Your spending last month exceeded $5000. Consider reviewing your expenses.");
            }

            // Analyze savings
            var totalSaved = transactions.Where(t => t.Type == TransactionType.Deposit).Sum(t => t.Amount);
            if (totalSaved < 500) // Example threshold
            {
                CreateInsight(userId, "Saving", "Your savings last month were below $500. Consider setting aside more funds.");
            }

            // Analyze spending by category
            //var entertainmentSpending = transactions.Where(t => t.Category == "Entertainment").Sum(t => t.Amount);
            //if (entertainmentSpending > 1000) // Example threshold
            //{
            //    CreateInsight(userId, "Spending", "Your entertainment expenses last month exceeded $1000. Consider setting a budget.");
            //}

            // Check for frequent large transactions
            var largeTransactions = transactions.Where(t => t.Amount > 2000).Count();
            if (largeTransactions > 3) // Example threshold
            {
                CreateInsight(userId, "Spending", "You had multiple large transactions last month. Ensure they are all authorized.");
            }

            // Compare with previous month's spending
            var previousMonthTransactions = _transactionService.GetRecentTransactionsForUser(userId, DateTime.UtcNow.AddMonths(-2));
            var previousMonthSpent = previousMonthTransactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);
            if (totalSpent > previousMonthSpent * 1.5) // 50% increase in spending
            {
                CreateInsight(userId, "Spending", "Your spending last month was 50% higher than the previous month. Review your purchases.");
            }

            _context.SaveChanges();
        }

        private void CreateInsight(int userId, string type, string description)
        {
            var insight = new FinancialInsight
            {
                UserId = userId,
                DateGenerated = DateTime.UtcNow,
                InsightType = type,
                Description = description,
                IsRead = false
            };

            _context.FinancialInsights.Add(insight);
        }

        public IEnumerable<FinancialInsight> GetInsightsForUser(int userId)
        {
            return _context.FinancialInsights.Where(i => i.UserId == userId).OrderByDescending(i => i.DateGenerated).ToList();
        }

    }
}
