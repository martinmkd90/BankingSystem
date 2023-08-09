using Banking.Domain.Enums;
using Banking.Domain.Models;

namespace Banking.Services.Interfaces
{
    public interface IScheduledPaymentService
    {
        void ProcessScheduledPayments();
       Task<IEnumerable<ScheduledPayment>> GetSchedulePayments(int userId);
        ScheduledPayment SchedulePayment(int fromAccountId, int toAccountId, double amount, DateTime startDate, RecurrenceType recurrence);
    }
}
