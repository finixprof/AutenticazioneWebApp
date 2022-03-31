using Autenticazione.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Views
{
    public class ProfileViewModel : Utente
    {
        public IFormFile FileImgProfilo { get; set; }

        public ProfileViewModel(Utente utente)
        {
            Id = utente.Id;
            Username = utente.Username;
            Email = utente.Email;
            PersonaId = utente.PersonaId;
            Persona = utente.Persona;
        }
    }
}
