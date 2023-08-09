using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface ITransferService
    {
        Transfer MakeTransfer(int fromAccountId, int toAccountId, double amount);
        IEnumerable<Transfer> GetTransfersForAccount(int accountId);
    }
}
