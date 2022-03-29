using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticazione.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DataCreazione { get; set; }

        public DateTime DataUltimaModifica { get; set; }
    }
}
