using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Helpers
{
    public static class PathHelper
    {
        public static string GetUrlToConfirmEmail(HttpRequest request, string token)
        {
            return $"{request.Scheme}://{request.Host.Value}/home/confirmEmail?token={token}";
        }


        public static string WebRootPath { get; set; }

        public static string GetPathUploads()
        {
            return $"{WebRootPath}\\wwwroot\\uploads";
        }


        public static string GetPathPersona(int id)
        {
            return $"{WebRootPath}\\wwwroot\\uploads\\persone\\{id}";
        }
    }
}
