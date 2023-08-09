using Banking.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public int UserId { get; set; }

        [NotMapped]
        public virtual User User { get; set; } = new User();
        public virtual AccountTypes AccountType { get; set; } = new AccountTypes();
        public virtual AccountOverview AccountOverview { get; set; } = new AccountOverview();
        public ICollection<Transfer> IncomingTransfers { get; set; } = new List<Transfer>();
        public ICollection<Transfer> OutgoingTransfers { get; set; } = new List<Transfer>();
        public int OverdraftLimit { get; set; }
        public bool IsPrimary { get; set; }
    }
}
