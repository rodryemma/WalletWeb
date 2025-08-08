using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.ExternalServices.Api.Models
{
    internal class PuenteNetModel
    {
        public string s { get; set; }
        public string errmsg { get; set; }
        public List<int> t { get; set; }
        public List<double> c { get; set; }
        public List<double> o { get; set; }
        public List<double> h { get; set; }
        public List<double> l { get; set; }
        public List<int> v { get; set; }
        public string nextTime { get; set; }

    }
}
