using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IInvestmentService
    {
        Investment Invest(int userId, string instrumentType, string instrumentName, double amount);
        void UpdateInvestmentValue(int investmentId, double newValue);
        Task<IEnumerable<Investment>> GetUserInvestments(int userId);
        IEnumerable<InvestmentHistory> GetInvestmentHistory(int investmentId);
    }
}
