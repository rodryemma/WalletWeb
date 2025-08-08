using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Api.Models
{
     class Cotizacion
    {
        public string Symbol { get; set; }
        public string Cotiza { get; set; }
    }

    static class CotizacionList
    {
        public static List<Cotizacion> Cotizacion { get; set; }
    }
}
