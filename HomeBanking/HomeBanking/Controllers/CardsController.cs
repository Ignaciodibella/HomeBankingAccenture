using HomeBanking.Migrations;
using HomeBanking.Models;
using HomeBanking.Models.Enums;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardRepository _cardRepository;
        private IClientRepository _clientRepository;

        public CardsController(ICardRepository cardRepository, IClientRepository clientRepository)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
        }

        [HttpPost("clients/current/cards")]
        public IActionResult CreateCurrentCard([FromBody] Card card) //Crea y asinga una tarjeta al usuario logueado en la sesión actual.
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

                int numberOfCards = currentClient.Cards.Where(c => c.Type == card.Type).Count();
                if (numberOfCards >= 3)
                {
                    return StatusCode(403, $"El cliente alcanzó la máxima cantidad de tarjetas {card.Type}");
                }

                Card newCard = new Card
                {
                    //Card Number y Cvv se generan aleatoriamente desde el constructor.
                    ClientId = currentClient.Id,
                    CardHolder = $"{currentClient.FirstName} {currentClient.LastName}",
                    Type = card.Type.ToString(),
                    Color = card.Color.ToString(),
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(4),
                };
                _cardRepository.Save(newCard);

                return Created("Tarjeta creada exitosamente", newCard);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrentCards()
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

                var userCards = _cardRepository.GetCardsByClient(currentClient.Id);
                return Ok(userCards);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
