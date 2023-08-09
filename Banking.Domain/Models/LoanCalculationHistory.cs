
namespace Banking.Domain.Models
{
    public class LoanCalculationHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public LoanCalculationRequest Request { get; set; }
        public LoanCalculationResult Result { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
