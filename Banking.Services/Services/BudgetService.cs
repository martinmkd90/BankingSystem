using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;

namespace Banking.Services.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly BankingDbContext _context;

        public BudgetService(BankingDbContext context)
        {
            _context = context;
        }

        public Budget CreateBudget(int userId, string category, double monthlyLimit)
        {
            var budget = new Budget
            {
                UserId = userId,
                Category = category,
                MonthlyLimit = monthlyLimit,
                AmountSpentThisMonth = 0
            };

            _context.Budgets.Add(budget);
            _context.SaveChanges();

            return budget;
        }

        public void UpdateBudget(int budgetId, double newLimit)
        {
            var budget = _context.Budgets.Find(budgetId);
            if (budget != null)
            {
                budget.MonthlyLimit = newLimit;
                _context.SaveChanges();
            }
        }

        public void DeleteBudget(int budgetId)
        {
            var budget = _context.Budgets.Find(budgetId);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Budget> GetBudgetsForUser(int userId)
        {
            return _context.Budgets.Where(b => b.UserId == userId).ToList();
        }

        public void TrackSpending(int userId, string category, double amount)
        {
            var budget = _context.Budgets.FirstOrDefault(b => b.UserId == userId && b.Category == category);
            if (budget != null)
            {
                budget.AmountSpentThisMonth += amount;
                _context.SaveChanges();
            }
        }
    }

}
