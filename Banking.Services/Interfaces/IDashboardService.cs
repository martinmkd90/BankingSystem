using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Dashboard> GetUserDashboard(int userId);
    }
}
