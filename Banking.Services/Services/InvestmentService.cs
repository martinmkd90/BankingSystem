using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly BankingDbContext _context;

        public InvestmentService(BankingDbContext context)
        {
            _context = context;
        }

        public Investment Invest(int userId, string instrumentType, string instrumentName, double amount)
        {
            var investment = new Investment
            {
                UserId = userId,
                InstrumentType = instrumentType,
                InstrumentName = instrumentName,
                AmountInvested = amount,
                InvestmentDate = DateTime.UtcNow,
                CurrentValue = amount // Initially, the current value is the invested amount
            };

            _context.Investments.Add(investment);
            _context.SaveChanges();

            return investment;
        }

        public void UpdateInvestmentValue(int investmentId, double newValue)
        {
            var investment = _context.Investments.SingleOrDefault(i => i.Id == investmentId);
            if (investment != null)
            {
                var history = new InvestmentHistory
                {
                    InvestmentId = investmentId,
                    Date = DateTime.UtcNow,
                    Value = newValue
                };

                investment.CurrentValue = newValue;
                _context.InvestmentHistories.Add(history);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Investment>> GetUserInvestments(int userId)
        {
            return await _context.Investments.Where(i => i.UserId == userId).AsNoTracking().ToListAsync();
        }

        public IEnumerable<InvestmentHistory> GetInvestmentHistory(int investmentId)
        {
            return _context.InvestmentHistories
                           .Where(ih => ih.InvestmentId == investmentId)
                           .OrderBy(ih => ih.Date)
                           .AsNoTracking()
                           .ToList();
        }
    }
}
