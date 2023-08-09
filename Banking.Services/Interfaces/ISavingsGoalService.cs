using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface ISavingsGoalService
    {
        SavingsGoal CreateSavingsGoal(int userId, string goalName, double targetAmount, DateTime targetDate);
        void UpdateSavingsGoal(int goalId, double newTargetAmount);
        IEnumerable<SavingsGoal> GetSavingsGoalsForUser(int userId);
        void DeleteSavingsGoal(int goalId);
    }

}
