using System.Collections;
using System.Collections.Generic;

namespace HomeBanking.Models
{
    public class Client
    {
        //prop (atajo)
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //Convención 4 EFC
        public ICollection<Account> Accounts { get; set; } //Un cliente puede tener mas de una cuenta.
    }
}

