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
                var clientVictor = context.Clients.FirstOrDefault(
                    c => c.Email == "vcoronado@gmail.com");
                if (clientVictor != null) //Si encontramos al cliente con mail "vcoronado@gmail.com"
                {
                    var accountsVictor = new Account[]
                    {
                        new Account{
                            Number = string.Empty, 
                            CreationDate = System.DateTime.Now,
                            Balance = 0,
                            ClientId = clientVictor.Id//le asociamos una cuenta con estos datos.
                        }, 
                        new Account
                        {
                            Number = "VIN001",
                            CreationDate = System.DateTime.Now,
                            Balance = -1000,
                            ClientId = clientVictor.Id
                        }
                    };
                    foreach (var account in accountsVictor) 
                    {
                        context.Accounts.Add(account);
                    }
                    //context.SaveChanges();
                }

                var clientIgnacio = context.Clients.FirstOrDefault(c => c.Email == "ignacio.dibella.n@gmail.com");
                if (clientIgnacio != null) 
                {
                    var accountsIgnacio = new Account[]
                    {
                        new Account{
                            Number = "CC01", 
                            CreationDate= System.DateTime.Now,
                            Balance = 50000, 
                            ClientId = clientIgnacio.Id
                        },
                        new Account{
                            Number = "CC02",
                            CreationDate=System.DateTime.Now,
                            Balance=1000000,
                            ClientId = clientIgnacio.Id
                        }
                    };
                    foreach (var account in accountsIgnacio)
                    {
                        context.Accounts.Add(account);
                    }
                    //context.SaveChanges();
                }
            }
            context.SaveChanges();
        }
    }
}
