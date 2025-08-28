using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Entites
{
    public class CuentaWallet
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Divisa { get; set; }
        public int DivisaId { get; set; }
    }
}
