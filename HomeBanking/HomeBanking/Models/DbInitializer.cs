using System;
using System.Linq;
using HomeBanking.Models.Enums;

namespace HomeBanking.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context) 
        {
            //Carga de datos de prueba en nuestra entidad Clients
            //#################################################################################################
            //Consultar si tenemos datos (d prueba).
            if (!context.Clients.Any()) 
            { 
                //Creamos datos de prueba
                var clients = new Client[]
                {
                    new Client{
                    FirstName = "Victor",
                    LastName = "Coronado",
                    Email = "vcoronado@gmail.com",
                    Password = "123456",
                    },
                    new Client{
                    FirstName = "Ignacio",
                    LastName = "Di Bella",
                    Email = "ignacio.dibella.n@gmail.com",
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
            //#################################################################################################
            if (!context.Accounts.Any())
            {
                var clientVictor = context.Clients.FirstOrDefault(
                    c => c.Email == "vcoronado@gmail.com");
                if (clientVictor != null) //Si encontramos al cliente con mail "vcoronado@gmail.com"
                {
                    var accountsVictor = new Account[]
                    {
                        /*
                        new Account{
                            Number = string.Empty, 
                            CreationDate = System.DateTime.Now,
                            Balance = 0,
                            ClientId = clientVictor.Id//le asociamos una cuenta con estos datos.
                        }, */
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
                    context.SaveChanges();
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
                    context.SaveChanges();
                }
            }

            //Carga de datos de prueba en nuestra entidad Transactions:
            //#################################################################################################
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(ac => ac.Number == "VIN001");

                if (account1 != null) 
                {
                    var transactions1 = new Transaction[]
                    {
                        new Transaction{
                            AccountId = account1.Id,
                            Amount = -2000,
                            Date = System.DateTime.Now.AddHours(-6),
                            Description = "Compra Nakaoutdoors",
                            Type = TransactionType.DEBIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = 10000,
                            Date =  System.DateTime.Now.AddHours(-7),
                            Description = "Transferencia Recibida",
                            Type = TransactionType.CREDIT.ToString()

                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = -500,
                            Date = System.DateTime.Now.AddHours(-8),
                            Description = "Compra en tiendaMia",
                            Type = TransactionType.DEBIT.ToString()
                        }
                    };

                    foreach (Transaction transaction in transactions1)
                    { 
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }

                var account2 = context.Accounts.FirstOrDefault(ac => ac.Number == "CC01");
                if (account2 != null)
                {
                    var transactions2 = new Transaction[]
                    {
                        new Transaction{
                            AccountId = account2.Id,
                            Amount = 5000,
                            Date = System.DateTime.Now.AddHours(-4),
                            Description = "Transferencia Recibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account2.Id,
                            Amount = 100000,
                            Date =  System.DateTime.Now.AddHours(-5),
                            Description = "Plazo Fijo Liquidado",
                            Type = TransactionType.CREDIT.ToString()

                        },
                    };

                    foreach (Transaction transaction in transactions2) 
                    {
                        context.Transactions.Add (transaction);
                    }
                    context.SaveChanges();
                }
            }

            //Carga de datos de prueba en nuestra entidad Loans:
            //#################################################################################################
            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                    {
                        new Loan{
                            Name = "Hipotecario",
                            MaxAmount = 500000,
                            Payments = "12,24,36,48,60"
                        },
                        new Loan{
                            Name = "Personal",
                            MaxAmount = 100000,
                            Payments = "6,12,24"
                        },
                        new Loan{
                            Name = "Automotriz",
                            MaxAmount = 300000,
                            Payments = "6,12,24,36"
                        }
                    };

                foreach (Loan loan in loans) 
                {
                    context.Loans.Add (loan);
                }
                context.SaveChanges();

                //Carga de datos de prueba en nuestra entidad Client Loans:
                //#################################################################################################

                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clienLoan2 = new ClientLoan
                        {
                            Amount = 500000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clienLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }
            }

            //Carga de datos de prueba en nuestra entidad Cards:
            //#################################################################################################

            if (!context.Cards.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if(client1 != null)
                {
                    var cards = new Card[]
                    {
                       new Card{
                           ClientId = client1.Id,
                           CardHolder = client1.FirstName + " " + client1.LastName,
                           Type = CardType.DEBIT.ToString(),
                           Color = CardColor.GOLD.ToString(),
                           Number =  "3325-6745-7876-4445",
                           Cvv = 990,
                           FromDate = DateTime.Now,
                           ThruDate = DateTime.Now.AddYears(4),
                       },
                        new Card{ 
                            ClientId = client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number =  "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(5),
                        }
                    };

                    foreach (Card card in cards)
                    { 
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
            }

        }
    }
}
