using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.DTOs
{
    public class AccountLinkingRequest
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccessToken { get; set; }  // This would typically be obtained after OAuth2 authentication with the third-party aggregator.
        public string RefreshToken { get; set; }  // Similarly, this would be obtained after OAuth2 authentication.
    }

}
