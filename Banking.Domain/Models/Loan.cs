using Banking.Domain.Enums;

namespace Banking.Domain.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; } = new LoanType();
        public double Amount { get; set; }
        public double OutstandingAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LoanStatus Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public string? RejectionReason { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
