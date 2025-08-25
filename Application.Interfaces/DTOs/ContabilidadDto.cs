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
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Categoria es obligatorio.")]
        public string Categoria { get; set; }

        //[Required(ErrorMessage = "El campo Cuenta es obligatorio.")]
        public string Cuenta { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double CantidadDivisa { get; set; }

        //[Required(ErrorMessage = "El campo Divisa es obligatorio.")]
        public string Divisa { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El campo Tipo Movimiento es obligatorio.")]
        public string TipoMovimiento { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double ValorCCL { get; set; }
        public double MontoUsd { get; set; }

        [Required(ErrorMessage = "El campo Categoria es obligatorio.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo Divisa es obligatorio.")]
        public int DivisaId { get; set; }

        [Required(ErrorMessage = "El campo Cuenta es obligatorio.")]
        public int CuentaWalletId { get; set; }
    }

    public class ContabilidadCreateDto
    {

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double CantidadDivisa { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El campo Tipo Movimiento es obligatorio.")]
        public string TipoMovimiento { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double ValorCCL { get; set; }

        [Required(ErrorMessage = "El campo Categoria es obligatorio.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo Divisa es obligatorio.")]
        public int DivisaId { get; set; }

        [Required(ErrorMessage = "El campo Cuenta es obligatorio.")]
        public int CuentaWalletId { get; set; }
    }

    public class ContabilidadUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double CantidadDivisa { get; set; }

        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El campo Tipo Movimiento es obligatorio.")]
        public string TipoMovimiento { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public double ValorCCL { get; set; }

        [Required(ErrorMessage = "El campo Categoria es obligatorio.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo Divisa es obligatorio.")]
        public int DivisaId { get; set; }

        [Required(ErrorMessage = "El campo Cuenta es obligatorio.")]
        public int CuentaWalletId { get; set; }
    }
}
