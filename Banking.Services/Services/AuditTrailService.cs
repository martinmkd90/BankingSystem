using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly BankingDbContext _context;

        public AuditTrailService(BankingDbContext context)
        {
            _context = context;
        }

        public void RecordAction(string action, string description, int userId)
        {
            var auditRecord = new AuditRecord
            {
                Action = action,
                Description = description,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };

            _context.AuditRecords.Add(auditRecord);
            _context.SaveChanges();
        }

        public IEnumerable<AuditRecord> GetAllAuditRecords()
        {
            return _context.AuditRecords.OrderByDescending(a => a.Timestamp).AsNoTracking().ToList();
        }


        public IEnumerable<AuditRecord> GetAuditsForUser(int userId)
        {
            return _context.AuditRecords.Where(ar => ar.UserId == userId).AsNoTracking().ToList();
        }
    }
}
