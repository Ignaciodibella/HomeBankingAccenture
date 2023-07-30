using System;

namespace HomeBanking.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        //Convención 4 EFC:
        public long AccountId { get; set; }
        public Account Account { get; set; } //Una transacción es de una cuenta
    }
}
