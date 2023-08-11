using Banking.Domain.Enums;

namespace Banking.Domain.Models
{
    public class LoanTransaction
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = new Loan();
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public LoanTransactionType Type { get; set; }
        public string Description { get; set; }
    }

}
