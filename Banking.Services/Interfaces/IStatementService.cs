using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IStatementService
    {
        AccountStatement GenerateMonthlyStatement(int accountId);
    }
}
