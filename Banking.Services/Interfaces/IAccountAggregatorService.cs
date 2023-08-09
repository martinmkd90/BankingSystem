using Banking.Domain.Models;
using Banking.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IAccountAggregatorService
    {
        Task<IEnumerable<ExternalAccount>> GetAllExternalAccountsForUser(int userId);
        Task LinkExternalAccount(int userId, AccountLinkingRequest request);
        Task RefreshAccountData(int userId, int externalAccountId);

    }

}
