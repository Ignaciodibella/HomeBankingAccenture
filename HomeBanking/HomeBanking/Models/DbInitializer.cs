using System.Linq;

namespace HomeBanking.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context) 
        {
            //Carga de datos de prueba en nuestra entidad Client
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
                    FirstName = "Victor",
                    LastName = "Coronado",
                    Email = "vcoronado@gmail.com",
                    Password = "123456",
                    },
                };

                foreach (var client in clients) 
                {
                    context.Clients.Add(client);
                    context.SaveChanges();
                }

                
            }
            //En caso de existir datos no hacemos nada.

            //Carga de datos de prueba en nuestra entidad Accounts:
            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(
                    c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null) //Si encontramos al cliente con mail "vcoronado@gmail.com"
                {
                    var accounts = new Account[]
                    {
                        new Account{Number = string.Empty, CreationDate = System.DateTime.Now,
                        Balance = 0, ClientId = accountVictor.Id} //le asociamos una cuenta con estos datos.
                    };
                    foreach (var account in accounts) 
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
