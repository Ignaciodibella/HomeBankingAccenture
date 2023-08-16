using HomeBanking.Data;
using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id).Include(account => account.Transactions).FirstOrDefault();
        }
        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll().Include(account => account.Transactions).ToList();
        }
        public void Save(Account account)
        {
            if (account.Id == 0) //El objeto tendrá un Id 0 si aún no fue guardado en la BD, por eso se lo crea.
            {
                Create(account);
            }
            else // si Id != 0, entonces ya existe en la BD y se procede a actualizarlo.
            { 
                Update(account);
            }
            SaveChanges();
        }
        public IEnumerable<Account> GetAccountsByClient(long clientId)
        { 
            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions).ToList();
        }
        public Account FindByNumber(string number)
        {
            return FindByCondition(account => account.Number.ToUpper() == number.ToUpper())
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }
    }
}
