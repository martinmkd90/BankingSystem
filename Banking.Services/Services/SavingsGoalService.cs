using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;

namespace Banking.Services.Services
{
    public class SavingsGoalService : ISavingsGoalService
    {
        private readonly BankingDbContext _context;

        public SavingsGoalService(BankingDbContext context)
        {
            _context = context;
        }

        public SavingsGoal CreateSavingsGoal(int userId, string goalName, double targetAmount, DateTime targetDate)
        {
            var savingsGoal = new SavingsGoal
            {
                UserId = userId,
                GoalName = goalName,
                TargetAmount = targetAmount,
                CurrentAmount = 0,
                TargetDate = targetDate
            };

            _context.SavingsGoals.Add(savingsGoal);
            _context.SaveChanges();

            return savingsGoal;
        }

        public void UpdateSavingsGoal(int goalId, double newTargetAmount)
        {
            var goal = _context.SavingsGoals.Find(goalId);
            if (goal != null)
            {
                goal.TargetAmount = newTargetAmount;
                _context.SaveChanges();
            }
        }

        public void DeleteSavingsGoal(int goalId)
        {
            var goal = _context.SavingsGoals.Find(goalId);
            if (goal != null)
            {
                _context.SavingsGoals.Remove(goal);
                _context.SaveChanges();
            }
        }

        public IEnumerable<SavingsGoal> GetSavingsGoalsForUser(int userId)
        {
            return _context.SavingsGoals.Where(sg => sg.UserId == userId).ToList();
        }

        public void ContributeToGoal(int goalId, double amount)
        {
            var goal = _context.SavingsGoals.Find(goalId);
            if (goal != null)
            {
                goal.CurrentAmount += amount;
                _context.SaveChanges();
            }
        }
    }

}
