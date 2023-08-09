using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class StatementService : IStatementService
    {
        private readonly BankingDbContext _context;

        public StatementService(BankingDbContext context)
        {
            _context = context;
        }

        public AccountStatement GenerateMonthlyStatement(int accountId)
        {
            var account = _context.Accounts.SingleOrDefault(a => a.Id == accountId);
            if (account == null)
                return null;

            var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var transactions = _context.Transactions
                .Where(t => t.AccountId == accountId && t.Date >= startDate && t.Date <= endDate)
                .AsNoTracking().ToList();

            var statement = new AccountStatement
            {
                AccountId = accountId,
                StartDate = startDate,
                EndDate = endDate,
                Transactions = transactions
            };

            return statement;
        }
    }

}
