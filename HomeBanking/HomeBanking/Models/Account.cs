using System;
using System.Collections.Generic;

namespace HomeBanking.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Number { get; set; } 
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }

        //Convención 4 EFC
        public long ClientId { get; set; }
        public Client Client { get; set; } //Una cuenta tiene un Cliente.
        public ICollection<Transaction> Transactions { get; set; } //Un cuenta puede tener mas de una transacción.

    }
}
