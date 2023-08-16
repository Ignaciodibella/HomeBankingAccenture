using HomeBanking.dtos;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HomeBanking.Models;
using System.Linq;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using HomeBanking.Helpers;
using System.Text.RegularExpressions;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, IPasswordHasher passwordHasher) //Constructor
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        
                        Accounts = client.Accounts.Select(ac => new AccountDTO
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),

                        Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                        { 
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),

                        Cards = client.Cards.Select(cc => new CardDTO
                        { 
                            Id = cc.Id,
                            CardHolder = cc.CardHolder,
                            Color = cc.Color,
                            Cvv = cc.Cvv,
                            FromDate = cc.FromDate,
                            ThruDate = cc.ThruDate,
                            Type = cc.Type,
                        }).ToList()
                    };

                    clientsDTO.Add(newClientDTO);
                }

                return Ok(clientsDTO);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try 
            {
                var client = _clientRepository.FindById(id);
                if (client == null) //Si no lo encuentro/no existen.
                {
                    //return Forbid();
                    return NotFound();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,

                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),

                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),

                    Cards = client.Cards.Select(cc => new CardDTO
                    {
                        Id = cc.Id,
                        CardHolder = cc.CardHolder,
                        Color = cc.Color,
                        Cvv = cc.Cvv,
                        FromDate = cc.FromDate,
                        ThruDate = cc.ThruDate,
                        Type = cc.Type,
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent() 
        {
            try 
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                { 
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null) 
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,

                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),

                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),

                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientRegistrationDTO clientRegistrationDTO) 
        {
            try
            { 
                //Validación de campos:
                if (String.IsNullOrEmpty(clientRegistrationDTO.Email) || String.IsNullOrEmpty(clientRegistrationDTO.Password) 
                    || String.IsNullOrEmpty(clientRegistrationDTO.FirstName) || String.IsNullOrEmpty(clientRegistrationDTO.LastName)) //Teniendo la validación de almenos 3 caracteres se pueden sacar las validaciones de FirstName y LastName
                {
                    return StatusCode(403, "Datos Inválidos");
                }

                //Validaciones Extra:
                if (clientRegistrationDTO.FirstName.Length < 3 || clientRegistrationDTO.LastName.Length < 3)
                {
                    return StatusCode(403, "El nombre y apellido deben poseer al menos 3 caracteres cada uno.");
                }

                //Caracteres especiales o numéricos en nombre y/o apellido:
                if (HasSpecialChars(clientRegistrationDTO.FirstName) || HasSpecialChars(clientRegistrationDTO.LastName))
                {
                    return StatusCode(403, "El nombre y apellido no puede contener caracteres especiales ni numéricos.");
                }

                //Cumplimiento de formato de password
                Regex passwordValidation = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");  //Para validar que incluya caracteres especiales se debe agregar (?=.*?[#?!@$%^&*-])
                if (! passwordValidation.IsMatch(clientRegistrationDTO.Password))
                {
                    return StatusCode(403, "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número.");
                }

                //Cumplimiento de formato de email:
                Regex emailValidation = new Regex("^\\S+@\\S+\\.\\S+$");
                if (!emailValidation.IsMatch(clientRegistrationDTO.Email))
                {
                    return StatusCode(403, "Email Inválido.");
                }

                var passwordHashed = _passwordHasher.Hash(clientRegistrationDTO.Password);

                //Verificamos que el cliente no exista:
                Client user = _clientRepository.FindByEmail(clientRegistrationDTO.Email);
                if (user != null)
                {
                    return StatusCode(403, "Email ya registado");
                }

                Client newClient = new Client
                {
                    Email = clientRegistrationDTO.Email,
                    Password = passwordHashed, //Guardamos la clave hasheada, no en texto plano.
                    FirstName = clientRegistrationDTO.FirstName,
                    LastName = clientRegistrationDTO.LastName
                };
                _clientRepository.Save(newClient);

                Client currentNewClient = _clientRepository.FindById(newClient.Id);

                //Creamos una cuenta al registrar un nuevo cliente.
                Account newAccount = new Account
                {
                    Number = "VIN-" + GenerateRandomNumber(8),
                    CreationDate = System.DateTime.Now,
                    Balance = 0,
                    ClientId = currentNewClient.Id
                };
                _accountRepository.Save(newAccount);

                //currentNewClient = _clientRepository.FindByEmail(clientRegistrationDTO.Email); //al vicio
                return Created("", currentNewClient);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GenerateRandomNumber(int digits)
        {
            Random random = new Random();
            string number = "";

            for (int i = 0; i < digits; i++)
            {
                number += random.Next(0, 10).ToString();
            }

            return number;
        }
        private bool HasSpecialChars(string yourString)
        {
            return yourString.Any(ch => !char.IsLetter(ch) && !char.IsWhiteSpace(ch));
        }

    }
}
