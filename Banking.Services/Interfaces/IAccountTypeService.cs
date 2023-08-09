using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IAccountTypeService
    {
        IEnumerable<AccountTypes> GetAllAccountTypes();
        AccountTypes GetAccountType(int id);
    }
}
