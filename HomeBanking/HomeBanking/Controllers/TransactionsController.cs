using HomeBanking.dtos;
using HomeBanking.Models;
using HomeBanking.Models.Enums;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
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
                if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                {
                    return Forbid("Ambas cuentas deben ser proporcionadas.");
                }
                if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                {
                    return Forbid("La transaccion debe ser a una cuenta distinta a la de origen.");
                }

                if(transferDTO.Description == string.Empty || transferDTO.Amount == 0) 
                {
                    return Forbid("Monto o descripcion no proporcionados.");
                }

                //Chequeo en cuenta origen (Existencia)
                Account fromAccount =_accountRepository.FindByNumber(transferDTO.FromAccountNumber);
                if (fromAccount == null)
                {
                    return Forbid("La cuenta origen no existe.");
                }
                //Chequeo en cuenta origen (Fondos Suficientes):
                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return Forbid("Fondos insuficientes.");
                }

                //Chequo en cuenta destino (Existencia):
                Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
                if (toAccount == null) 
                {
                    return Forbid("La cuenta destino no existe.");
                }

                //Transacción DEBITO:
                _transactionRepository.Save(new Transaction 
                {
                    Type = TransactionType.DEBIT.ToString(),
                    Amount = transferDTO.Amount * (-1),
                    Description = transferDTO.Description + " " + toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });

                //Transacción CREDITO:
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = transferDTO.Amount,
                    Description = transferDTO.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });

                fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
                _accountRepository.Save(fromAccount);

                toAccount.Balance = toAccount.Balance + transferDTO.Amount;
                _accountRepository.Save(toAccount);

                return Created("Transaccion efectuada con exito", fromAccount);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
