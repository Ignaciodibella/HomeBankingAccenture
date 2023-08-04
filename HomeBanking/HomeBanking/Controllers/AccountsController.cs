using HomeBanking.dtos;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository) //Constructor
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet("accounts")]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountsDTO = new List<AccountDTO>();

                foreach (Account account in accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,

                        Transactions = account.Transactions.Select(t => new TransactionDTO
                        {
                            Id = t.Id,
                            Type = t.Type,
                            Amount = t.Amount,
                            Description = t.Description,
                            Date = t.Date,
                        }).ToList()
                    };

                    accountsDTO.Add(newAccountDTO);
                }

                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return NotFound();
                }

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,

                    Transactions = account.Transactions.Select(t => new TransactionDTO
                    {
                        Id= t.Id,
                        Type = t.Type,
                        Amount = t.Amount,
                        Description = t.Description,
                        Date = t.Date,
                    }).ToList()
                };

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("clients/current/accounts")]
        public IActionResult PostAccountToCurrent()
        {
            try 
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null)
                {
                    return Forbid();
                }

                if (currentClient.Accounts.Count >= 3)
                {
                    return StatusCode(403, "El cliente ya posee 3 cuentas.");
                }

                //Validar que no esté asignado ese nro de cuenta.
                //Corregir. Hacer lo del nro en el constructor.
                Random random = new Random();
                string randomAccountNumber = $"VIN-{random.Next(100000, 100000).ToString()}";

                Account newAccount = new Account
                {
                    Number = randomAccountNumber,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = currentClient.Id
                };
                _accountRepository.Save(newAccount);
                return Created("Cuanta creada exitosamente", newAccount);

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("clients/current/accounts")]
        public IActionResult GetCurrentAccounts() 
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }
                Client currentClient = _clientRepository.FindByEmail(email);
                if (currentClient == null)
                {
                    return Forbid();
                }

                var userAccounts = _accountRepository.GetAccountsByClient(currentClient.Id);

                return Ok(userAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
    
}
