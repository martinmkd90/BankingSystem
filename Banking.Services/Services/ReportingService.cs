using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Banking.Services.Services
{
    public class ReportingService : IReportingService
    {
        private readonly BankingDbContext _context;
        private readonly IMemoryCache _cache;

        public ReportingService(BankingDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IEnumerable<Transaction> GetMonthlyStatement(int accountId, DateTime month)
        {
            try
            {
                var cacheKey = $"MonthlyStatement_{accountId}_{month}";
                if (!_cache.TryGetValue(cacheKey, out List<Transaction>? transactions))
                {
                    transactions = FilterTransactionsByDate(accountId, month, month.AddMonths(1)).ToList();
                    _cache.Set(cacheKey, transactions, TimeSpan.FromHours(1)); // Cache for 1 hour
                }
                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Transaction> GetTransactionsAboveAverage(int accountId)
        {
            var averageTransactionAmount = _context.Transactions.Where(t => t.AccountId == accountId).Average(t => t.Amount);
            return _context.Transactions.Where(t => t.AccountId == accountId && t.Amount > averageTransactionAmount).AsNoTracking().ToList();
        }

        public IEnumerable<Account> GetOverdraftAccountsReport()
        {
            return _context.Accounts.Where(a => a.Balance < 0).AsNoTracking().ToList();
        }

        // Get a report of all loans that are overdue
        public IEnumerable<Loan> GetOverdueLoansReport()
        {
            var currentDate = DateTime.UtcNow;
            return _context.Loans.Where(l => l.EndDate < currentDate && l.Status == LoanStatus.Approved && l.OutstandingAmount > 0).AsNoTracking().ToList();
        }

        // Get a report of all transactions in a given date range
        public IEnumerable<Transaction> GetTransactionsReport(DateTime startDate, DateTime endDate)
        {
            return _context.Transactions.Where(t => t.Date >= startDate && t.Date <= endDate).OrderByDescending(t => t.Date).AsNoTracking().ToList();
        }

        // Get a report of all users who have not performed any transactions in the last 'n' days
        public IEnumerable<User> GetInactiveUsersReport(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return _context.Users.Where(u => !u.Transactions.Any(t => t.Date > cutoffDate)).AsNoTracking().ToList();
        }

        // Get a report of the top 'n' users with the highest account balances
        public IEnumerable<User> GetTopUsersByBalanceReport(int topN)
        {
            return _context.Users.OrderByDescending(u => u.Accounts.Sum(a => a.Balance)).Take(topN).AsNoTracking().ToList();
        }

        // Get a report of all scheduled payments that failed in the last 'n' days due to insufficient funds
        public IEnumerable<ScheduledPayment> GetFailedScheduledPaymentsReport(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return _context.ScheduledPayments.Where(sp => sp.LastPaymentDate > cutoffDate && sp.FailReason == FailReason.InsufficientFunds).AsNoTracking().ToList();
        }

        public IEnumerable<Transaction> GetTransactionsByCategory(int accountId, string category)
        {
            return _context.Transactions
                .Where(t => t.AccountId == accountId && t.Category == category)
                .OrderByDescending(t => t.Date)
                .ToList();
        }
        private IQueryable<Transaction> FilterTransactionsByDate(int accountId, DateTime startDate, DateTime endDate)
        {
            return _context.Transactions
                .Where(t => t.AccountId == accountId && t.Date >= startDate && t.Date <= endDate);
        }

    }

}
