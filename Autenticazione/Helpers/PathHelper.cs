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
    }
}
