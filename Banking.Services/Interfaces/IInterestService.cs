using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IInterestService
    {
        double CalculateInterest(int accountId, DateTime fromDate, DateTime toDate);
        void ApplyInterest(int accountId);
    }
}
