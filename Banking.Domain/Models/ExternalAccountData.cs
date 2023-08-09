using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class ExternalAccountData
    {
        public double Balance { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<Transaction> RecentTransactions { get; set; }
    }
}
