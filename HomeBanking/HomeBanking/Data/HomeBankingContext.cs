using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Data
{
    public class HomeBankingContext : DbContext
    {
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }

        //dBSet
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<ClientLoan> ClientLoans { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
