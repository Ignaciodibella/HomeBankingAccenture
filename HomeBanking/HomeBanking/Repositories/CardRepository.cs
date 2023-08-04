using HomeBanking.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) 
        { }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }

        public Card FindById(long id) 
        {
            return FindByCondition(card => card.Id == id).FirstOrDefault();        
        }

        public IEnumerable<Card> GetCardsByClient(long clientId) 
        {
            return FindByCondition(card => card.ClientId == clientId).ToList();
        }
    }
}