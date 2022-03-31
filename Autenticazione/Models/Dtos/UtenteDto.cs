using Autenticazione.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Dtos
{
    public class UtenteDto : Utente
    {
        public IFormFile FileImgProfilo { get; set; }

        public new string  Password { get; set; }

    }
}
