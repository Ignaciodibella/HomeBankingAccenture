using HomeBanking.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBanking.dtos
{
    public class ClientDTO
    {
        [JsonIgnore] //Evita que se devuelva visualmente el Id.
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; } //Paso las cuentas en formato DTO para evitar recursividad
                                                              //al usar el GetAllClients() en el controlador del cliente.
        public ICollection<ClientLoanDTO> Loans { get; set; }
    }
}
