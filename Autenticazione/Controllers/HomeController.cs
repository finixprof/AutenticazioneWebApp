using Autenticazione.Helpers;
using Autenticazione.Models;
using Autenticazione.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Autenticazione.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string token)
        {
            token = token.Replace(" ", "+"); //questo è un workaround
            var tokenInChiaro = HttpUtility.HtmlDecode(CryptoHelper.Decrypt(token));
            var tokenPieces = tokenInChiaro.Split("_");
            var id = tokenPieces[0];
            var email = tokenPieces[1];
            try
            {
                DatabaseHelper.ConfirmEmail(id, email);
            }
            catch (Exception ex)
            {
                ViewData["MsgKo"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var msgKo = "Completa tutti i campi nella maniera corretta<br>";
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var msgKoAggregate = errors.Select(t => t.ErrorMessage).Aggregate((x, y) => $"{x}<br>{y}");
                ViewData["MsgKo"] = msgKo + msgKoAggregate;

            }
            if (!model.IsPrivacy)
            {
                ViewData["MsgKo"] = "Accetta i termini della privacy";
            }
            if (DatabaseHelper.ExistUserWithEmail(model.Email))
            {
                ViewData["MsgKo"] = "Esiste già un utente con questo indirizzo email";
            }
            if (ViewData["MsgKo"] != null)
                return View(model);

            model.Password = CryptoHelper.HashSHA256(model.Password);
            var utente = DatabaseHelper.SaveUtente(model);
            model.Id = utente.Id;
            var tokenInChiaro = $"{model.Id}_{model.Email}";
            var token = HttpUtility.HtmlEncode(CryptoHelper.Encrypt(tokenInChiaro));
            var link = PathHelper.GetUrlToConfirmEmail(HttpContext.Request, token);
            try
            {
                EmailHelper.Send(utente, link);
            }
            catch (Exception ex)
            {
                ViewData["MsgKo"] = ex.Message;
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
