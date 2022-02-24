using Autenticazione.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Views
{
    public class SignInViewModel : Utente
    {
        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Conferma password")]
        [DataType(DataType.Password)]
        public string ConfermaPassword { get; set; }
    }
}
