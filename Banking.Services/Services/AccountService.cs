using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingDbContext _context;
        private readonly ITransactionService _transactionService;
        
        public AccountService(BankingDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.SingleOrDefault(a => a.Id == id);
        }

        public async Task<AccountOverview> GetAccountOverview(int accountId)
        {
            return await _context.AccountOverviews.SingleOrDefaultAsync(ao => ao.AccountId == accountId);
        }


        public bool Deposit(int accountId, double amount)
        {
            var account = _context.Accounts.SingleOrDefault(a => a.Id == accountId);
            if (account == null)
                return false;

            account.Balance += amount;
            _context.SaveChanges();

            _transactionService.RecordTransaction(accountId, amount, TransactionType.Debit);

            return true;
        }

        public bool Withdraw(int accountId, double amount)
        {
            var account = _context.Accounts.SingleOrDefault(a => a.Id == accountId);
            if (account == null || account.Balance < amount)
                return false;

            account.Balance -= amount;
            _context.SaveChanges();

            _transactionService.RecordTransaction(accountId, amount, TransactionType.Withdrawal);

            return true;
        }
        public double GetBalance(int userId)
        {
            return _context.Accounts.FirstOrDefault(a => a.UserId == userId)?.Balance ?? 0;
        }

        public void DeductAmount(int userId, double amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.UserId == userId);
            if (account != null)
            {
                account.Balance -= amount;
                _context.SaveChanges();
            }
        }

    }
}
