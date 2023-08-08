using HomeBanking.dtos;
using HomeBanking.Models;
using HomeBanking.Models.Enums;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;
        private string toAccountNumber;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, 
                               ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository,
                               ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult GetLoans()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();

                foreach (Loan loan in loans)
                {
                    var newLoanDTO = new LoanDTO
                    {
                        Id = loan.Id,
                        Name = loan.Name,
                        MaxAmount = loan.MaxAmount,
                        Payments = loan.Payments,
                    };

                    loansDTO.Add(newLoanDTO);
                }
                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SubscribeToLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                //Verificar que la cuenta destino pertenezca al cliente autenticado
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid("Debe ingresar un Email.");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("No existe el cliente");
                }

                if (loanApplicationDTO.Amount == null || loanApplicationDTO.Amount <= 0)
                {
                    return Forbid(); //403
                }

                if (loanApplicationDTO.Payments == "0" || loanApplicationDTO.Payments == string.Empty)
                {
                    return Forbid();
                }

                var selectedLoan = _loanRepository.FindById(loanApplicationDTO.LoanId);
                if (selectedLoan == null) //El préstamo no existe.
                {
                    return Forbid();
                }

                if (loanApplicationDTO.Amount >= selectedLoan.MaxAmount) //El monto solicitado excede el del préstamo.
                {
                    return Forbid();
                }

                if (loanApplicationDTO.Payments.Contains(selectedLoan.Payments))
                {
                    return Forbid(); //Cantidad de cueotas no coincide con el plan.
                }

                

                var selectedAccount = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                if (selectedAccount == null)
                {
                    return Forbid("La cuenta proporcionada no existe");
                }

                if (selectedAccount.ClientId != client.Id)
                {
                    return Forbid("La cuenta proporcionada no le pertenece");
                }

                _clientLoanRepository.Save(new ClientLoan
                {
                    LoanId = loanApplicationDTO.LoanId,
                    ClientId = client.Id,
                    Amount = loanApplicationDTO.Amount * (1.20),
                    Payments = loanApplicationDTO.Payments
                });

                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = loanApplicationDTO.Amount,
                    Description = selectedLoan.Name + " loan approved",
                    AccountId = selectedAccount.Id,
                    Date = DateTime.Now,
                });

                selectedAccount.Balance = selectedAccount.Balance + loanApplicationDTO.Amount;
                _accountRepository.Save(selectedAccount);

                return Created("Operación realizada con éxito", selectedAccount); //201

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
