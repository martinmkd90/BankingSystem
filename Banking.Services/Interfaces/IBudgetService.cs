using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IBudgetService
    {
        Budget CreateBudget(int userId, string category, double monthlyLimit);
        void UpdateBudget(int budgetId, double newLimit);
        IEnumerable<Budget> GetBudgetsForUser(int userId);
        void DeleteBudget(int budgetId);
    }

}
