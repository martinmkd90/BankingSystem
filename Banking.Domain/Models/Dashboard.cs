namespace Banking.Domain.Models
{
    public class Dashboard
    {
        public User User { get; set; } = new User();
        public AccountOverview AccountOverview { get; set; } = new AccountOverview();
        public IEnumerable<Loan>? ActiveLoans { get; set; }
        public IEnumerable<Investment>? Investments { get; set; }
        public IEnumerable<ScheduledPayment>? UpcomingPayments { get; set; }
        public DateTime? LastLogin { get; set; } = new DateTime();
        public IEnumerable<Notification>? UnreadNotifications { get; set; }
    }

}
