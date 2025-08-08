using Domain.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DolarService : IDolarService
    {
        public string DolarCCL { get; set; } = string.Empty;
    }
}
