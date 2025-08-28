using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Model.Entites;
using Microsoft.AspNetCore.Mvc;

namespace UI.WalletWeb.Controllers
{
    public class TransferenciaController : Controller
    {
        ITransferenciaService _transferenciaService;

        public TransferenciaController(ITransferenciaService transferenciaService)
        {
            _transferenciaService = transferenciaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("transferencia/json")]
        public async Task<IActionResult> GetTransferencia(DateTime xFechaDesde)
        {
            var transferencia = await _transferenciaService.ObtenerTransferenciaJoinDBFullAsyncService(xFechaDesde);
            if (!transferencia.Success) { return BadRequest(transferencia.Message); }

            var lista = transferencia.Data.Select(x => new
            {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.CuentaEnvia,
                x.CuentaEnviaId,
                x.CuentaRecibe,
                x.CuentaRecibeId,
                x.Comentario,
                x.Monto
            }).ToList();
            
            return Json(lista);

        }

        [HttpPost("transferencia/crear")]
        public async Task<IActionResult> Insertar([FromBody] TransferenciaCreateDto transferencia)
        {
            //TODO : Validar que no exista el nombre
            if (transferencia == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            // Validaciones y lógica
            var editTransferencia = new Transferencia()
            {
                Fecha = transferencia.Fecha,
                CuentaEnviaId = transferencia.CuentaEnviaId,
                CuentaRecibeId = transferencia.CuentaRecibeId,
                Comentario = transferencia.Comentario,
                Monto = transferencia.Monto
            };

            var transacciones = await _transferenciaService.InsertarTransferenciaAsyncService(editTransferencia);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("transferencia/eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarDto eliminar)
        {
            if (eliminar == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }
            var transaccion = await _transferenciaService.EliminarTransferenciaAsyncService(eliminar.Id);
            return transaccion.Success ?
                Ok() :
                BadRequest(transaccion.Message);
        }

        [HttpPost("transferencia/editar")]
        public async Task<IActionResult> Editar([FromBody] TransferenciaUpdateDto transferencia)
        {
            if (transferencia == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            // Validaciones y lógica
            var editTransferencia = new Transferencia()
            {
                Id = transferencia.Id,
                Fecha = transferencia.Fecha,
                CuentaEnviaId = transferencia.CuentaEnviaId,
                CuentaRecibeId = transferencia.CuentaRecibeId,
                Comentario = transferencia.Comentario,
                Monto = transferencia.Monto
            };

            var transacciones = await _transferenciaService.EditarTransferenciaAsyncService(editTransferencia);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }
    }
}
