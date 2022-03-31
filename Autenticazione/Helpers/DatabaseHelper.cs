using Autenticazione.Models.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Autenticazione.Helpers
{
    public static class DatabaseHelper
    {

        public static string ConnectionString { get; set; }


        public static Utente SaveUtente(Utente model)
        {
            if (model.Id == 0)
            {
                return InsertUtente(model);
            }
            else
            {
                return UpdateUtente(model);
            }
        }

        private static Utente UpdateUtente(Utente model)
        {
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "UPDATE utente " +
                    "SET personaid=@personaid, " +
                    "dataultimamodifica=@dataultimamodifica  " +
                    "WHERE id = @id";

                var affectedRows = db.Execute(sqlQuery, model);
                if (affectedRows == 1)
                    return model;
            }
            throw new Exception("Errore aggiornamento non completato");
        }

        private static Utente InsertUtente(Utente model)
        {
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "INSERT INTO utente (username,email,password,isprivacy) VALUES (@username,@email,@password,1); " +
                        "SELECT LAST_INSERT_ID()";
                model.Id = db.Query<int>(sqlQuery, model).FirstOrDefault();
                if (model.Id > 0)
                    return model;
            }
            throw new Exception("Errore inserimento non completato");
        }

        public static Utente Login(string username, string password)
        {
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "SELECT * FROM utente " +
                    " WHERE dataUltimaModifica IS NOT NULL and " +
                    " (email=@username OR username=@username) and password=@password";

                var utente = db.Query<Utente>(sqlQuery, new { username = username, password = password }).FirstOrDefault();
                return utente;
            }
        }

        public static void ConfirmEmail(string id, string email)
        {
            var data = DateTime.Now;
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "UPDATE utente " +
                    " SET dataUltimaModifica=@data  " +
                    " WHERE id = @id and email=@email";

                var affectedRows = db.Execute(sqlQuery, new { data = data, id = id, email = email });
                if (affectedRows != 1)
                    throw new Exception("Errore conferma email");

            }
        }

        public static bool ExistUserWithEmail(string email)
        {
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "SELECT * FROM utente " +
                    " WHERE dataUltimaModifica IS NOT NULL and email=@email";

                var utente = db.Query<Utente>(sqlQuery, new { email = email }).FirstOrDefault();
                if (utente != null)
                    return true;

            }
            return false;
        }

        public static Utente SaveUtentePersona(Utente utente, bool isInsert = false)
        {
            if (isInsert)
            {
                utente.Persona = InsertPersona(utente.Persona);
                utente.PersonaId = utente.Persona.Id;
                utente.DataUltimaModifica = utente.Persona.DataUltimaModifica;
                UpdateUtente(utente);
            }
            else
            {
                UpdatePersona(utente.Persona);
            }
            return utente;
        }

        private static Persona UpdatePersona(Persona persona)
        {
            persona.DataUltimaModifica = DateTime.Now;
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "UPDATE persona " +
                    "SET nome=@nome, " +
                    "cognome=@cognome  " +
                    "sesso=@sesso  " +
                    "dataNascita=@dataNascita  " +
                    "imgProfilo=@imgProfilo  " +
                    "dataultimamodifica=@dataultimamodifica  " +
                    "WHERE id = @id";

                var affectedRows = db.Execute(sqlQuery, persona);
                if (affectedRows == 1)
                    return persona;
            }
            throw new Exception("Errore aggiornamento non completato");
        }

        private static Persona InsertPersona(Persona persona)
        {
            persona.DataUltimaModifica = DateTime.Now;
            using (var db = new MySqlConnection(ConnectionString))
            {
                var sqlQuery = "INSERT INTO persona (nome,cognome,dataNascita,sesso,imgProfilo,dataUltimaModifica) VALUES (@nome,@cognome,@dataNascita,@sesso,@imgProfilo,@dataUltimaModifica); " +
                        "SELECT LAST_INSERT_ID()";
                persona.Id = db.Query<int>(sqlQuery, persona).FirstOrDefault();
                if (persona.Id > 0)
                    return persona;
            }
            throw new Exception("Errore inserimento non completato");
        }

    }
}
