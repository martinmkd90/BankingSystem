using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Models
{
    public class AccountOverview
    {
        [Key]
        public int AccountId { get; set; }
        public double CurrentBalance { get; set; } = new double();
        public double AvailableBalance { get; set; } = new double();
        public int RecentTransactionCount { get; set; } = new int();
        public Account Account { get; set; } = new Account();
        public string AccountStatus { get; set; } = string.Empty; // e.g., "Good Standing", "Overdrawn", etc.

        [NotMapped]
        public List<string> Alerts { get; set; } = new List<string>(); // e.g., "Overdraft protection is off", "Unusual activity detected", etc.
    }

}