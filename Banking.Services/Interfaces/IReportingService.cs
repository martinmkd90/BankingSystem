using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IReportingService
    {
        IEnumerable<Transaction> GetMonthlyStatement(int accountId, DateTime month);
        IEnumerable<Transaction> GetTransactionsAboveAverage(int accountId);

    }
}
