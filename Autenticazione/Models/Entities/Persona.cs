using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Entities
{
    public class Persona : BaseEntity
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Display(Name = "Immagine profilo")]
        public string ImgProfilo { get; set; }
        public bool Sesso { get; set; }
        public DateTime DataNascita { get; set; }

    }
}
