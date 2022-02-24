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
                var sqlQuery = "UPDATE nazione SET nome=@nome  WHERE id = @id";

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
    }
}
