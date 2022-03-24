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
            ViewData["Username"] = HttpContext.Session.GetString("username");
            var utente = HttpContext.Session.GetObject<Utente>("utenteLoggato");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");
        }
    }
}
