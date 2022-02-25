using Autenticazione.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Autenticazione.Helpers
{
    public static class EmailHelper
    {
        public static string HostSmtp { get; set; }

        public static int PortSmtp { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }

        public static void Send(Utente utente, string link)
        {
            //HostSmtp = "smtp-mail.outlook.com";
            //Email = "buspascal5d2021@outlook.it";
            //Password = "CalabreseErrante2021";
            //PortSmtp = 587;
            //configurazione del client smtp
            var smtpClient = new SmtpClient(HostSmtp)
            {
                Port = PortSmtp,
                Credentials = new NetworkCredential(Email, Password),
                EnableSsl = true,
            };
            //preparazione della mail
            var mailMessage = new MailMessage
            {
                From = new MailAddress(Email),
                Subject = "AUTENTICAZIONE APP: Conferma indirizzo mail per registrazione portale",
                Body = $"<h1>Ciao {utente.Username}</h1>" +
                    $"<a href='{link}'>clicca qui per confermar</a>",
                IsBodyHtml = true,
            };
            //aggiunta destinatario/i
            mailMessage.To.Add(utente.Email);

            try
            {
                //invio della mail
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore invio mail");
            }
        }
    }
}
