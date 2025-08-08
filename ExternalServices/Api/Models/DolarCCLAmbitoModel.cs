using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Api.Models
{
    internal class DolarCCLAmbitoModel
    {
        public string compra { get; set; }
        public string venta { get; set; }
        public string fecha { get; set; }
        public string ultimo { get; set; }
        public string valor { get; set; }
        public string variacion { get; set; }

        [JsonProperty("class-variacion")]
        public string classvariacion { get; set; }

    }
}
