using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IAccountService
    {
        Account GetAccount(int id);
        bool Deposit(int accountId, double amount);
        bool Withdraw(int accountId, double amount);
        double GetBalance(int userId);
        void DeductAmount(int userId, double amount);
        Task<AccountOverview> GetAccountOverview(int userId);
    }
}
