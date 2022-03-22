using Autenticazione.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Views
{
    public class LoginViewModel 
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
