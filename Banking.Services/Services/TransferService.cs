using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class TransferService : ITransferService
    {
        private readonly BankingDbContext _context;
        private readonly ITransactionService _transactionService;

        public TransferService(BankingDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        public Transfer MakeTransfer(int fromAccountId, int toAccountId, double amount)
        {
            var fromAccount = _context.Accounts.SingleOrDefault(a => a.Id == fromAccountId);
            var toAccount = _context.Accounts.SingleOrDefault(a => a.Id == toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new InvalidOperationException("Invalid account details.");

            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var transfer = new Transfer
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Date = DateTime.UtcNow
            };

            _context.Transfers.Add(transfer);
            _context.SaveChanges();

            _transactionService.RecordTransaction(fromAccountId, amount, TransactionType.TransferOut);
            _transactionService.RecordTransaction(toAccountId, amount, TransactionType.TransferIn);

            return transfer;
        }


        public IEnumerable<Transfer> GetTransfersForAccount(int accountId)
        {
            return _context.Transfers.Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId).AsNoTracking().ToList();
        }
    }

}
