using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class ExternalAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AccessToken { get; set; }  // Encrypted
        public string RefreshToken { get; set; }  // Encrypted
    }
}
