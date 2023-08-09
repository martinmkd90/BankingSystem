using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IAdminService
    {
        int GetTotalUsers();
        double GetTotalBankBalance();
        IEnumerable<Transaction> GetFlaggedTransactions();
        IEnumerable<User> GetInactiveUsers();
    }
}
