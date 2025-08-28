using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TransferenciaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Cuenta envio es obligatorio.")]
        public int CuentaEnviaId { get; set; }

        [Required(ErrorMessage = "El campo Fecha Cuenta es obligatorio.")]
        public int CuentaRecibeId { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }
    }

    public class TransferenciaCreateDto
    {
        //Fecha se crea automaticamente en base de datos
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Cuenta envio es obligatorio.")]
        public int CuentaEnviaId { get; set; }

        [Required(ErrorMessage = "El campo Cuenta recibío es obligatorio.")]
        public int CuentaRecibeId { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }
    }

    public class TransferenciaUpdateDto
    {
        [Required(ErrorMessage = "El campo Id es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Cuenta envio es obligatorio.")]
        public int CuentaEnviaId { get; set; }

        [Required(ErrorMessage = "El campo Fecha Cuenta es obligatorio.")]
        public int CuentaRecibeId { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }
    }
}
