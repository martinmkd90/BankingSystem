using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class OverdraftService : IOverdraftService
    {
        private readonly BankingDbContext _context;
        private readonly INotificationService _notificationService;

        public OverdraftService(BankingDbContext context)
        {
            _context = context;
        }

        public bool AllowOverdraft(int accountId, double withdrawalAmount)
        {
            var account = _context.Accounts.Include(a => a.AccountType).SingleOrDefault(a => a.Id == accountId);
            if (account == null)
                return false;

            if (account.AccountType.Id != (int)AccountType.Checking)
                return false;

            var effectiveBalance = account.Balance - withdrawalAmount;
            bool withinOverdraftLimit = effectiveBalance >= -account.OverdraftLimit;

            if (withinOverdraftLimit && effectiveBalance < 0)
            {
                ApplyOverdraftFee(account);
                NotifyUser(account.UserId, "Your account has gone into overdraft.");
                RecordOverdraftUsage(accountId, withdrawalAmount, effectiveBalance);
            }

            return withinOverdraftLimit;
        }

        private void ApplyOverdraftFee(Account account)
        {
            // Assuming a flat fee for simplicity. This can be enhanced.
            const double overdraftFee = 35.00;
            account.Balance -= overdraftFee;
            _context.SaveChanges();
        }

        private void NotifyUser(int userId, string message)
        {
           _notificationService.SendNotification(userId, message);
        }

        private void RecordOverdraftUsage(int accountId, double withdrawalAmount, double effectiveBalance)
        {
            // Record the overdraft usage for reporting or analytics.
            var overdraftRecord = new OverdraftHistory
            {
                AccountId = accountId,
                Date = DateTime.UtcNow,
                WithdrawalAmount = withdrawalAmount,
                EffectiveBalance = effectiveBalance
            };
            _context.OverdraftHistories.Add(overdraftRecord);
            _context.SaveChanges();
        }


    }
}
