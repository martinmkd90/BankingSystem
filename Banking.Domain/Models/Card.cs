using Banking.Domain.Enums;

namespace Banking.Domain.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public CardType CardType { get; set; }
        public string CardNumber { get; set; } // Should be encrypted
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
