using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface IAuditTrailService
    {
        public void RecordAction(string action, string description, int userId);
        IEnumerable<AuditRecord> GetAuditsForUser(int userId);
    }
}
