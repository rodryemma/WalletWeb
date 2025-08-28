using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Entites
{
    public class Transferencia
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string CuentaEnvia { get; set; }
        public int CuentaEnviaId { get; set; }
        public string CuentaRecibe { get; set; }
        public int CuentaRecibeId { get; set; }
        public decimal Monto { get; set; }
        public string Comentario { get; set; }
    }
}
