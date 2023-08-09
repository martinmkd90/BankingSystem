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
    public class AccountTypeService : IAccountTypeService
    {
        private readonly BankingDbContext _context;

        public AccountTypeService(BankingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AccountTypes> GetAllAccountTypes()
        {
            return _context.AccountTypes.AsNoTracking().AsNoTracking().ToList();
        }

        public AccountTypes GetAccountType(int id)
        {
            return _context.AccountTypes.SingleOrDefault(at => at.Id == id);
        }
    }
}
