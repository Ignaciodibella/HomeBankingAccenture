using HomeBanking.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ICardRepository
    {
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> GetCardsByClient(long clientId);
    }
}
