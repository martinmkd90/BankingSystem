using Banking.Data.Context;
using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Services
{
    public class CardService : ICardService
    {
        private readonly BankingDbContext _context;

        public CardService(BankingDbContext context)
        {
            _context = context;
        }

        public Card RequestCard(int userId, CardType cardType)
        {
            var card = new Card
            {
                UserId = userId,
                CardType = cardType,
                CardNumber = GenerateCardNumber(), // This method should generate a unique card number
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                IsActive = false
            };

            _context.Cards.Add(card);
            _context.SaveChanges();

            return card;
        }

        public Card? ActivateCard(int cardId)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
            if (card == null)
                return null;

            card.IsActive = true;
            _context.SaveChanges();

            return card;
        }

        public Card BlockCard(int cardId)
        {
            var card = _context.Cards.SingleOrDefault(c => c.Id == cardId);
            if (card == null)
                return null;

            card.IsActive = false;
            _context.SaveChanges();

            return card;
        }

        public IEnumerable<Card> GetUserCards(int userId)
        {
            return _context.Cards.Where(c => c.UserId == userId).AsNoTracking().ToList();
        }

        private string GenerateCardNumber()
        {
            // Fixed Issuer Identification Number (IIN)
            string iin = "123456";

            // Generate the next 9 digits randomly
            Random random = new();
            string accountIdentifier = "";
            for (int i = 0; i < 9; i++)
            {
                accountIdentifier += random.Next(0, 10).ToString();
            }

            // Combine IIN and account identifier
            string cardNumberWithoutChecksum = iin + accountIdentifier;

            // Calculate the checksum digit using the Luhn algorithm
            int checksumDigit = CalculateLuhnChecksumDigit(cardNumberWithoutChecksum);

            // Return the complete card number
            return cardNumberWithoutChecksum + checksumDigit;
        }

        private int CalculateLuhnChecksumDigit(string number)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum * 9) % 10;
        }

    }
}
