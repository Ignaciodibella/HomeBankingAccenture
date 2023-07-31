using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class ClientRepository: RepositoryBase<Client>, IClientRepository //ClientRepository Hereda de RepositoryBase para poder implementar los métodos allí definidos.
    {                                                                        //Por esto, en RepositoryBase definimos public abstract.
                                                                             //Luego, implementa el IClientRepository
        public ClientRepository(HomeBankingContext repositoryContext): base(repositoryContext)
        { }
        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .FirstOrDefault();
        }
        public IEnumerable<Client> GetAllClients() 
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .ToList();
        }
        public void Save(Client client) 
        {
            Create(client);
            SaveChanges();
        }
    }
}
