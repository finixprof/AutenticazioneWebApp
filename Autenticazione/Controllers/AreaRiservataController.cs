using Autenticazione.Helpers;
using Autenticazione.Helpers.Extensions;
using Autenticazione.Models.Dtos;
using Autenticazione.Models.Entities;
using Autenticazione.Models.Views;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Controllers
{
    [Authorize]
    public class AreaRiservataController : Controller
    {
        public IActionResult Index()
        {
            Utente utente = GetLoggedUser();
            if (utente == null)
                return RedirectToAction("login", "home"); //non servirà praticamente mai perchè l'utente non loggato viene bloccato da authorize
            if (utente.PersonaId == 0)
                return RedirectToAction("profile");
            return View();
        }

        private Utente GetLoggedUser()
        {
            var utente = HttpContext.Session.GetObject<Utente>("utenteLoggato");
            if (utente != null)
                ViewData["Username"] = utente.Username;
            return utente;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.SetObject("utenteLoggato", null);
            return RedirectToAction("index", "home");
        }

        public IActionResult Profile()
        {
            Utente utente = GetLoggedUser();
            if (utente==null)
                return RedirectToAction("login", "home");
            return View(new ProfileViewModel(utente));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(UtenteDto utenteDto) //metto un dto per il trasporto del file immagine
        {
            //1) faccio il controllo della validità
            if (!ModelState.IsValid)
            {
                var msgKo = "Completa tutti i campi nella maniera corretta<br>";
                var errors = ModelState.Values.SelectMany(v => v.Errors); //recuperiamo la lista di errori
                var msgKoAggregate = errors.Select(t => t.ErrorMessage).Aggregate((x, y) => $"{x}<br>{y}"); //concatena in una string gli errori
                ViewData["MsgKo"] = msgKo + msgKoAggregate;
                return View(new ProfileViewModel((Utente)utenteDto));
            }

            Utente utente = (Utente)utenteDto;
            //2) se è la prima volta, faccio la save per inserire la persona
            //   e aggiornare utente (personaid e dataultimamodifica)
            if (utente.PersonaId==0)
            {
                utente = DatabaseHelper.SaveUtentePersona(utente, true);
            }

            //3) se ho fatto l'upload dell'immagine la salvo.
            //   Vedi img bandiera in webapplication musei
            if (utenteDto.FileImgProfilo != null)
            {
                var path = PathHelper.GetPathPersona(utenteDto.PersonaId);
                if (!Directory.Exists(path))
                {
                    //1)creazione cartella persone/id in uploads se non esiste con id quello del modello
                    Directory.CreateDirectory(path);
                }
                //2)salvare il contenuto di FileImgProfilo nel percorso creato
                utente.Persona.ImgProfilo = Guid.NewGuid() + Path.GetExtension(utenteDto.FileImgProfilo.FileName);
                var filePath = path + "\\" + utente.Persona.ImgProfilo;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    try
                    {
                        utenteDto.FileImgProfilo.CopyTo(fileStream);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            //4) faccio l'update di persona (utente non viene toccato)
            utente = DatabaseHelper.SaveUtentePersona(utente);
            ViewData["MsgOk"] = "Profilo aggiornato";
            return View(new ProfileViewModel(utente));
        }
    }
}
