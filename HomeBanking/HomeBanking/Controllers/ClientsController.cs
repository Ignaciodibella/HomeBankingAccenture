using HomeBanking.dtos;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HomeBanking.Models;
using System.Linq;
using System;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository) //Constructor
        {
            _clientRepository = clientRepository;
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
        
    }
}
