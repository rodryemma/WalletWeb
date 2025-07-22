using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Entity
{
    public class Contabilidad
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Categoria { get; set; }
        public string Cuenta { get; set; }
        public double CantidadDivisa { get; set; }
        public string Divisa { get; set; }
        public string Comentario { get; set; }
        public string TipoMovimiento { get; set; }
        public double ValorCCL { get; set; }
    }
}
