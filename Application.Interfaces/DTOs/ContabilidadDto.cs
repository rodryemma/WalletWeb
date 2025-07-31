using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ContabilidadDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Categoria { get; set; }
        public string Cuenta { get; set; }
        public double MontoUsd { get; set; }
        public string Comentario { get; set; }
        public string TipoMovimiento { get; set; }
    }
}
