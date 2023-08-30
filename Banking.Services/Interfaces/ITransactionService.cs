using Banking.Domain.Enums;
using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public interface ITransactionService
    {
        Transaction RecordTransaction(int accountId, double amount, TransactionType type);
        List<Transaction> GetTransactions(int accountId);
        Task<PagedTransactionResponse> GetTransactionsForUser(int userId, int page, int pageSize);
        IEnumerable<Transaction> GetRecentTransactionsForUser(int userId, DateTime fromDate);
        void AddTransaction(Transaction transaction);
    }
}
