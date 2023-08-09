using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingDbContext _context;

        public TransactionService(BankingDbContext context)
        {
            _context = context;
        }

        public Transaction RecordTransaction(int accountId, double amount, TransactionType type)
        {
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = type,
                Date = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction;
        }

        public List<Transaction> GetTransactions(int accountId)
        {
            return _context.Transactions.Where(t => t.AccountId == accountId).AsNoTracking().ToList();
        }
        public List<Transaction> GetTransactionsForUser(int userId)
        {
            return _context.Transactions
                           .Where(t => _context.Accounts.Any(a => a.Id == t.AccountId && a.UserId == userId))
                           .AsNoTracking()
                           .ToList();
        }
        public IEnumerable<Transaction> GetRecentTransactionsForUser(int userId, DateTime fromDate)
        {
            return _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= fromDate)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            }

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
    }
}
