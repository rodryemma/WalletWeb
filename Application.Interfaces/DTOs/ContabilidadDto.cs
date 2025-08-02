using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ContabilidadDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public string Cuenta { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double CantidadDivisa { get; set; }

        [Required]
        public string Divisa { get; set; }

        [Required]
        public string Comentario { get; set; }

        [Required]
        public string TipoMovimiento { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double ValorCCL { get; set; }
        public double MontoUsd { get; set; }
    }
}
