using HomeBanking.Models;

namespace HomeBanking.Repositories
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoan clientLoan);
    }
}