using System;

namespace HomeBanking.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }

        //Convención 4:
        public long ClientId { get; set; }
        public Client client { get; set; }
    }
}
