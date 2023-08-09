using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class ScheduleNewPaymentService : IScheduledPaymentService
    {
        private readonly BankingDbContext _context;

        public ScheduleNewPaymentService(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScheduledPayment>> GetSchedulePayments(int userId)
        {
            return await _context.ScheduledPayments.Where(sp => sp.Id == userId).AsNoTracking().ToListAsync();
        }
        public ScheduledPayment SchedulePayment(int fromAccountId, int toAccountId, double amount, DateTime startDate, RecurrenceType recurrence)
        {
            var scheduledPayment = new ScheduledPayment
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                StartDate = startDate,
                Recurrence = recurrence
            };

            _context.ScheduledPayments.Add(scheduledPayment);
            _context.SaveChanges();

            return scheduledPayment;
        }

        public void ProcessScheduledPayments()
        {
            try
            {
                var paymentsDue = _context.ScheduledPayments
                    .Where(p => IsPaymentDue(p))
                    .AsNoTracking()
                    .ToList();

                foreach (var payment in paymentsDue)
                {
                    if (payment.Account.Balance >= payment.Amount)
                    {
                        // Deduct the amount from the source account
                        payment.Account.Balance -= payment.Amount;

                        // If the payment is to another account within the same system, credit the amount to the destination account
                        var destinationAccount = _context.Accounts.SingleOrDefault(a => a.Id == payment.ToAccountId);
                        if (destinationAccount != null)
                        {
                            destinationAccount.Balance += payment.Amount;
                        }

                        // Update the LastPaymentDate for the scheduled payment
                        payment.LastPaymentDate = DateTime.UtcNow;

                        // Create a transaction record for the payment
                        var transaction = new Transaction
                        {
                            AccountId = payment.FromAccountId,
                            Amount = payment.Amount,
                            Date = DateTime.UtcNow,
                            Type = TransactionType.ScheduledPayment,
                            Description = $"Scheduled payment to {payment.Recipient}"
                        };
                        _context.Transactions.Add(transaction);
                    }
                    else
                    {
                        NotifyUserOfInsufficientFunds(payment.Account.UserId, payment.Amount);
                        Console.WriteLine($"Failed to process scheduled payment for Account ID: {payment.FromAccountId} due to insufficient funds.");
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing scheduled payments: {ex.Message}");
            }
        }

        private static bool IsPaymentDue(ScheduledPayment payment)
        {
            DateTime? nextPaymentDate = null;

            switch (payment.Recurrence)
            {
                case RecurrenceType.Daily:
                    nextPaymentDate = payment.LastPaymentDate?.AddDays(1);
                    break;
                case RecurrenceType.Weekly:
                    nextPaymentDate = payment.LastPaymentDate?.AddDays(7);
                    break;
                case RecurrenceType.Monthly:
                    nextPaymentDate = payment.LastPaymentDate?.AddMonths(1);
                    break;
                    // Add other recurrence types as needed
            }

            return (payment.LastPaymentDate == null && DateTime.UtcNow >= payment.StartDate) ||
                   (nextPaymentDate.HasValue && DateTime.UtcNow >= nextPaymentDate.Value);
        }

        private void NotifyUserOfInsufficientFunds(int userId, double amount)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = $"Scheduled payment of ${amount} failed due to insufficient funds.",
                Date = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
        }
    }
}
    

