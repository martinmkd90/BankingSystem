using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;

namespace Banking.Services.Services
{
    public class AutomatedInvestmentService : IAutomatedInvestmentService
    {
        private readonly BankingDbContext _context;
        private readonly IAccountService _accountService;  // Assuming you have an account service to fetch user balances
        private readonly IInvestmentService _investmentService;
        private readonly INotificationService _notificationService;

        public AutomatedInvestmentService(BankingDbContext context, IAccountService accountService, IInvestmentService investmentService)
        {
            _context = context;
            _accountService = accountService;
            _investmentService = investmentService;
        }
        public AutomatedInvestmentRule GetRule(int ruleId)
        {
            return _context.AutomatedInvestmentRules.FirstOrDefault(r => r.Id == ruleId);
        }

        public AutomatedInvestmentRule CreateRule(AutomatedInvestmentRule rule)
        {
            _context.AutomatedInvestmentRules.Add(rule);
            _context.SaveChanges();
            return rule;
        }

        public AutomatedInvestmentRule UpdateRule(AutomatedInvestmentRule rule)
        {
            _context.AutomatedInvestmentRules.Update(rule);
            _context.SaveChanges();
            return rule;
        }

        public void DeleteRule(int ruleId)
        {
            var rule = _context.AutomatedInvestmentRules.Find(ruleId);
            if (rule != null)
            {
                _context.AutomatedInvestmentRules.Remove(rule);
                _context.SaveChanges();
            }
        }

        public IEnumerable<AutomatedInvestmentRule> GetUserRules(int userId)
        {
            return _context.AutomatedInvestmentRules.Where(r => r.UserId == userId).ToList();
        }

        public void ExecuteRules()
        {
            var activeRules = _context.AutomatedInvestmentRules.Where(r => r.IsActive).ToList();

            foreach (var rule in activeRules)
            {
                var userBalance = _accountService.GetBalance(rule.UserId);

                bool shouldInvest = false;

                switch (rule.TriggerType)
                {
                    case TriggerType.AccountBalanceThreshold:
                        shouldInvest = userBalance > rule.TriggerValue;
                        break;

                    case TriggerType.Monthly:
                        shouldInvest = DateTime.UtcNow.Day == 1;  // Assuming investment on the 1st of every month
                        break;

                    case TriggerType.OnSalaryDay:
                        shouldInvest = DateTime.UtcNow.Day == 25;  // Assuming salary day is the 25th of every month
                        break;
                }

                if (shouldInvest)
                {
                    if (userBalance >= rule.InvestmentAmount)
                    {
                        _investmentService.Invest(rule.UserId, rule.InvestmentInstrumentType, rule.InvestmentInstrument, rule.InvestmentAmount);
                        _accountService.DeductAmount(rule.UserId, rule.InvestmentAmount);
                    }
                    else
                    {
                        // Actual code to log or notify that the user doesn't have sufficient balance for the investment
                        _notificationService.SendNotification(rule.UserId, "Insufficient balance for automated investment.");
                    }
                }
            }
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
