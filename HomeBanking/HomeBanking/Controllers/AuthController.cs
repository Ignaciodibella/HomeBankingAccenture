using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using HomeBanking.Models;
using HomeBanking.dtos;
using HomeBanking.Helpers;

namespace HomeBanking.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private readonly IPasswordHasher _passwordHasher;
        public AuthController(IClientRepository clientRepository, IPasswordHasher passwordHasher)
        {
            _clientRepository = clientRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Client client)
        {
            try
            { 
                

                Client user = _clientRepository.FindByEmail(client.Email);
                bool result = _passwordHasher.Verify(user.Password, client.Password); //revisar (puede ser al revés)

                //if (user == null || !String.Equals(user.Password, client.Password)) - previo a usar hasheo.
                if (user == null || !result)
                {
                    return Unauthorized();
                }
                
                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            { 
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok();
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
