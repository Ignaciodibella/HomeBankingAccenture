using System;

namespace HomeBanking.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; } = string.Empty;
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }

        //Convención 4:
        public long ClientId { get; set; }
        public Client client { get; set; }

        public Card() //Constructor para asignar el número y Cvv aleatorios.
        {
            int digits = 16;
            string rawNumber = GenerateRandomNumber(digits);
            
            //Formateo el número de tarjeta para que tenga la forma XXXX-XXXX-XXXX-XXXX (debería hacerse en el front...).
            for (int i = 0; i < digits; i += 4)
                {
                Number += rawNumber.Substring(i, Math.Min(4, digits - i));
                if (i+4  < digits) 
                    {
                    Number += '-';
                    }
                }   

            Cvv = int.Parse(GenerateRandomNumber(3));
        }
        private string GenerateRandomNumber(int digits)
        {
            Random random = new Random();
            string number = "";

            for (int i = 0; i < digits; i++)
            {
                number += random.Next(0, 10).ToString();
            }
            number = number.PadRight(digits, '0'); //REVISAR, no está funcionando como se espera. PadLeft no sirve porque al pasarlo a int se pierde el cero incrustado.
            return number;
        }
    }
}
