using Banking.Domain.Enums;
using Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Interfaces
{
    public interface ICardService
    {
        Card RequestCard(int userId, CardType cardType);
        Card ActivateCard(int cardId);
        Card BlockCard(int cardId);
        IEnumerable<Card> GetUserCards(int userId);
    }
}
