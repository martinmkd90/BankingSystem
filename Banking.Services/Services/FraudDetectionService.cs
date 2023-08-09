using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;

namespace Banking.Services.Services
{
    public class FraudDetectionService : IFraudDetectionService
    {
        private readonly BankingDbContext _context;
        private readonly INotificationService _notificationService;

        public FraudDetectionService(BankingDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public void AnalyzeTransaction(Transaction transaction)
        {
            List<string> reasons = new List<string>();

            // Example: Flag transactions over a certain amount as suspicious
            if (transaction.Amount > 10000)
            {
                reasons.Add("High transaction amount");
            }

            // Check for rapid multiple transactions
            var recentTransactions = _context.Transactions
                .Where(t => t.UserId == transaction.UserId && t.Timestamp > DateTime.UtcNow.AddMinutes(-10))
                .ToList();

            if (recentTransactions.Count > 5) // This is just an example threshold
            {
                reasons.Add("Multiple transactions in a short time");
            }

            // Check for transactions in unusual locations
            var frequentLocations = _context.Transactions
                .Where(t => t.UserId == transaction.UserId)
                .GroupBy(t => t.Location)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            if (!frequentLocations.Contains(transaction.Location))
            {
                reasons.Add($"Unusual transaction location: {transaction.Location}");
            }

            // ... more patterns and checks can be added here

            if (reasons.Any())
            {
                var alert = new FraudAlert
                {
                    TransactionId = transaction.Id,
                    DetectedOn = DateTime.UtcNow,
                    Reason = string.Join(", ", reasons),
                    Status = FraudAlertStatus.Pending
                };

                _context.FraudAlerts.Add(alert);
                _context.SaveChanges();

                // Notify the bank staff or user about the suspicious transaction
                _notificationService.SendNotification(transaction.UserId, "Suspicious transaction detected: " + alert.Reason);
            }
        }

        public IEnumerable<FraudAlert> GetPendingAlerts()
        {
            return _context.FraudAlerts.Where(f => f.Status == FraudAlertStatus.Pending).ToList();
        }

        public void ResolveAlert(int alertId, string resolvedBy, FraudAlertStatus status)
        {
            var alert = _context.FraudAlerts.SingleOrDefault(f => f.Id == alertId);
            if (alert != null)
            {
                alert.Status = status;
                alert.ResolvedBy = resolvedBy;
                alert.ResolvedOn = DateTime.UtcNow;

                _context.SaveChanges();
            }
        }
    }
}
