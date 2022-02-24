using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Entities
{
    public class Utente
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int PersonaId { get; set; }

        [Required]
        public bool IsPrivacy { get; set; }

        public DateTime DataCreazione { get; set; }

        public DateTime DataUltimaModifica { get; set; }



    }
}
