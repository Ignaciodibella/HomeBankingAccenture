using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class ClientRepository: RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext): base(repositoryContext)
        { }
        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id).Include(client => client.Accounts).FirstOrDefault();
        }
        public IEnumerable<Client> GetAllClients() 
        {
            return FindAll().Include(client => client.Accounts).ToList();
        }
        public void Save(Client client) 
        {
            Create(client);
            SaveChanges();
        }
    }
}
