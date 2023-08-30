using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Banking.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        public TransactionService(BankingDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
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
        public async Task<PagedTransactionResponse> GetTransactionsForUser(int userId, int page, int pageSize)
        {
            var transactions = await _context.Transactions
                                       .Where(t => _context.Accounts.Any(a => a.Id == t.AccountId && a.UserId == userId))
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .AsNoTracking()
                                       .ToListAsync();

            var totalCount = _context.Transactions
                                     .Count(t => _context.Accounts.Any(a => a.Id == t.AccountId && a.UserId == userId));

            return new PagedTransactionResponse
            {
                Transactions = transactions,
                TotalCount = totalCount
            };
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
            try
            {
                _logger.LogInformation("Performing transaction");

                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                _logger.LogInformation("Transaction performed successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while performing the transaction", ex);
            }
        }
    }
}
