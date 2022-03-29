using Autenticazione.Helpers.Extensions;
using Autenticazione.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                return RedirectToAction("profilo");
            return View();
        }

        private Utente GetLoggedUser()
        {
            var utente = HttpContext.Session.GetObject<Utente>("utenteLoggato");
            ViewData["Username"] = utente.Username;
            return utente;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.SetObject("utenteLoggato", null);
            return RedirectToAction("index", "home");
        }

        public IActionResult Profilo()
        {
            Utente utente = GetLoggedUser();
            return View();
        }

        [HttpPost]
        public IActionResult Profilo(int id)
        {
            return View();
        }
    }
}
