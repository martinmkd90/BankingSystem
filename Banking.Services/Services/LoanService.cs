using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class LoanService : ILoanService
    {
        private readonly BankingDbContext _context;

        public LoanService(BankingDbContext context)
        {
            _context = context;
        }

        public Loan ApplyForLoan(LoanApplicationDto applicationDto)
        {
            var loan = new Loan
            {
                UserId = applicationDto.UserId,
                LoanTypeId = applicationDto.LoanType.Id, // Explicitly cast the enum to int
                Amount = applicationDto.AmountRequested,
                OutstandingAmount = applicationDto.AmountRequested,
                StartDate = DateTime.UtcNow,
                Status = LoanStatus.Pending
            };

            _context.Loans.Add(loan);
            _context.SaveChanges();

            return loan;
        }

        public Loan ApproveLoan(int loanId)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId);
            if (loan == null)
                return null;

            loan.Status = LoanStatus.Approved;
            _context.SaveChanges();
            SendNotification(loan.UserId, "Your loan application has been approved.");

            return loan;
        }

        public Loan RejectLoan(int loanId)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId);
            if (loan == null)
                return null;

            loan.Status = LoanStatus.Rejected;
            _context.SaveChanges();

            SendNotification(loan.UserId, "Your loan application has been rejected.");
            return loan;
        }
        public void ApplyInterest(int loanId)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == loanId) ?? throw new InvalidOperationException("Invalid loan ID.");
            var interestRate = loan.LoanType.InterestRate;
            var interestAmount = loan.OutstandingAmount * (interestRate / 100);

            loan.OutstandingAmount += interestAmount;

            // Add a transaction record for interest application
            var transaction = new LoanTransaction
            {
                LoanId = loan.Id,
                Amount = interestAmount,
                Date = DateTime.UtcNow,
                Type = LoanTransactionType.InterestApplication,
                Description = $"Interest applied for loan ID {loan.Id}"
            };
            _context.LoanTransactions.Add(transaction);  // Assuming you have a LoanTransactions DbSet

            _context.SaveChanges();
        }

        public bool MakeRepayment(LoanRepaymentDto repaymentDto)
        {
            var loan = _context.Loans.SingleOrDefault(l => l.Id == repaymentDto.LoanId);
            if (loan == null || loan.OutstandingAmount < repaymentDto.AmountPaid)
                throw new InvalidOperationException("Invalid repayment details.");

            loan.OutstandingAmount -= repaymentDto.AmountPaid;
            if (loan.OutstandingAmount == 0)
                loan.EndDate = DateTime.UtcNow;

            _context.SaveChanges();

            return true;
        }

        public async Task<IEnumerable<Loan>> GetUserLoans(int userId)
        {
            return await _context.Loans.Where(l => l.UserId == userId).AsNoTracking().ToListAsync();
        }

        private void SendNotification(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Date = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
        }
    }
}
