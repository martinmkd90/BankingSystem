using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
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
    public class AdminService : IAdminService
    {
        private readonly BankingDbContext _context;

        public AdminService(BankingDbContext context)
        {
            _context = context;
        }

        public int GetTotalUsers()
        {
            return _context.Users.Count();
        }

        public double GetTotalBankBalance()
        {
            return _context.Accounts.Sum(a => a.Balance);
        }

        public IEnumerable<Transaction> GetFlaggedTransactions()
        {
            // Real-world scenario: Flagged transactions could be based on a combination of factors like unusually large amounts, rapid succession of transactions, etc.
            var averageTransactionAmount = _context.Transactions.Average(t => t.Amount);
            return _context.Transactions.Where(t => t.Amount > averageTransactionAmount * 2).AsNoTracking().ToList();
        }

        public IEnumerable<User> GetInactiveUsers()
        {
            var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
            return _context.Users.Where(u => !_context.Transactions.Any(t => t.User.Id == u.Id && t.Date > threeMonthsAgo)).AsNoTracking().ToList();
        }

        public IEnumerable<Loan> GetAllPendingLoans()
        {
            return _context.Loans.Where(l => l.Status == LoanStatus.Pending).AsNoTracking().ToList();
        }

        public Loan? ApproveLoanByAdmin(int loanId)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId);
            if (loan == null)
                return null;

            loan.Status = LoanStatus.Approved;
            _context.SaveChanges();

            // Record this action in the audit trail
            RecordAction("Loan Approval", $"Loan with ID {loanId} approved.", loan.UserId);

            return loan;
        }

        public Loan? RejectLoanByAdmin(int loanId, string reason)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId);
            if (loan == null)
                return null;

            loan.Status = LoanStatus.Rejected;
            loan.RejectionReason = reason;
            _context.SaveChanges();

            // Record this action in the audit trail
            RecordAction("Loan Rejection", $"Loan with ID {loanId} rejected. Reason: {reason}", loan.UserId);

            return loan;
        }

        public IEnumerable<User> GetAllUsersWithOverdraft()
        {
            return _context.Users.Where(u => u.Accounts.Any(a => a.Balance < 0)).AsNoTracking().ToList();
        }

        public bool ToggleUserStatus(int userId, bool isActive)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
                return false;

            user.IsActive = isActive;
            _context.SaveChanges();

            // Record this action in the audit trail
            var action = isActive ? "User Activation" : "User Deactivation";
            RecordAction(action, $"User with ID {userId} {action.ToLower()}.", user.Id);

            return true;
        }

        private void RecordAction(string action, string description, int adminUserId)
        {
            var auditRecord = new AuditRecord
            {
                Action = action,
                Description = description,
                Timestamp = DateTime.UtcNow,
                UserId = adminUserId
            };

            _context.AuditRecords.Add(auditRecord);
            _context.SaveChanges();
        }
    }

}
