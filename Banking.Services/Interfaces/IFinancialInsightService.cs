using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IFinancialInsightService
    {
        void GenerateInsightsForUser(int userId);
        IEnumerable<FinancialInsight> GetInsightsForUser(int userId);
    }
}
