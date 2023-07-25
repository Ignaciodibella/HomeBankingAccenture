using System.Linq;

namespace HomeBanking.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context) 
        {
            //Consultar si tenemos datos (d prueba).
            if (!context.Clients.Any()) 
            { 
                //Creamos datos de prueba
                var clients = new Client[]
                { 
                    new Client{
                    FirstName = "Ignacio",
                    LastName = "Di Bella",
                    Email = "ignacio.dibella.n@gmail.com",
                    Password = "123456",
                    },
                    new Client{
                    FirstName = "Maximiliano",
                    LastName = "Di Bella",
                    Email = "maximilianodibella@gmail.com",
                    Password = "abcdefg",
                    },
                };

                foreach (var client in clients) 
                {
                    context.Clients.Add(client);
                    context.SaveChanges();
                }

                
            }
            //En caso de existir datos no hacemos nada.
        }
    }
}
