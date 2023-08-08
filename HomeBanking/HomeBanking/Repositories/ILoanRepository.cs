using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
