using HomeBanking.Models;
using System;

namespace HomeBanking.dtos
{
    public class AccountDTO //Por ahora = a Account.cs (por modificar)...
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
    }
}
