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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["MsgKo"] = "Username e password devono essere compilati";
                return View(model);
            }
            //faccio l'hash sha 256 della password
            model.Password = CryptoHelper.HashSHA256(model.Password);

            //cerco l'utente nel database
            var utente = DatabaseHelper.Login(model.Username, model.Password);
            if (utente == null)
            {
                ViewData["MsgKo"] = "Username e/o password non corretti";
                return View(model);
            }

            //Se non ci sono errori
            //1)metto in sessione l'utente

            //2)vado verso l'area riservata
            return RedirectToAction("Index","AreaRiservata");
        }


            public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string token)
        {
            //8) confermo la mail e completo registrazione.
            //Arrivo qui dal click sul link della mail inviata in fase di registrazione
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
            //1) controllo che i campi siano compilati correttamente
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
            //2 verifico che non esista l'utente
            if (ViewData["MsgKo"] == null && DatabaseHelper.ExistUserWithEmail(model.Email))
            {
                ViewData["MsgKo"] = "Esiste già un utente con questo indirizzo email";
            }
            //3) mando gli errori se ce ne sono
            if (ViewData["MsgKo"] != null)
                return View(model);
            //4) faccio l'hash sha 256 della password
            model.Password = CryptoHelper.HashSHA256(model.Password);
            //5) salvo l'utente
            var utente = DatabaseHelper.SaveUtente(model);
            model.Id = utente.Id;
            var tokenInChiaro = $"{model.Id}_{model.Email}";
            //6) creo il token per la mail
            var token = HttpUtility.HtmlEncode(CryptoHelper.Encrypt(tokenInChiaro));
            var link = PathHelper.GetUrlToConfirmEmail(HttpContext.Request, token);
            try
            {
                //7) invio la mail
                EmailHelper.Send(utente, link);
            }
            catch (Exception ex)
            {
                ViewData["MsgKo"] = ex.Message;
            }

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
