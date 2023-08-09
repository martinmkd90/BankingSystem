using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IFraudDetectionService
    {
        void AnalyzeTransaction(Transaction transaction);
        IEnumerable<FraudAlert> GetPendingAlerts();
        void ResolveAlert(int alertId, string resolvedBy, FraudAlertStatus status);
    }
}
