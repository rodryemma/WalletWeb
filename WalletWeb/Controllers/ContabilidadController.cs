using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.CodeDom;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UI.WalletWeb.Controllers
{
    public class ContabilidadController : Controller
    {
        IContabilidadService _contabilidaService;

        public ContabilidadController(IContabilidadService contabilidadService)
        {
            _contabilidaService = contabilidadService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("transacciones/json")]
        public async Task<IActionResult> GetTransacciones(string tipoMovimiento = "Total", string fecha = "2025-01-01")
        {
            var FechaObtenida = ValidationHelper.ValidarFecha(fecha);
            var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(tipoMovimiento.ToLower(), FechaObtenida);
            if (!transacciones.Success) { return BadRequest(transacciones.Message); }
            
            var lista = transacciones.Data.Select(x => new
            {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.Categoria,
                x.Cuenta,
                x.CantidadDivisa,
                x.Divisa,
                x.Comentario,
                x.TipoMovimiento,
                x.ValorCCL,
                x.MontoUsd,
                x.CategoriaId,
                x.DivisaId,
                x.CuentaWalletId
            }).ToList();
            return Json(new
            {
                data = lista
            });
        
        }

        [HttpPost("transacciones/editar")]
        public async Task<IActionResult> Editar([FromBody] ContabilidadUpdateDto transaccion)
        {
            if (transaccion == null)
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
            var editContabilidad = new Contabilidad()
            {
                Id = transaccion.Id,
                Fecha = transaccion.Fecha,
                CantidadDivisa = transaccion.CantidadDivisa,
                Comentario = transaccion.Comentario,
                TipoMovimiento = transaccion.TipoMovimiento,
                ValorCCL = transaccion.ValorCCL,
                DivisaId = transaccion.DivisaId,
                CuentaWalletId = transaccion.CuentaWalletId
            };

            var transacciones = await _contabilidaService.EditarContabilidadPersonalAsyncService(editContabilidad);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("transacciones/crear")]
        public async Task<IActionResult> Insertar([FromBody] ContabilidadCreateDto transaccion)
        {
            //TODO : Validar que no exista el nombre
            if (transaccion == null)
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
            var editContabilidad = new Contabilidad()
            {
                Fecha = transaccion.Fecha,
                CantidadDivisa = transaccion.CantidadDivisa,
                Comentario = transaccion.Comentario,
                TipoMovimiento = transaccion.TipoMovimiento,
                ValorCCL = transaccion.ValorCCL,
                CategoriaId = transaccion.CategoriaId,
                DivisaId = transaccion.DivisaId,
                CuentaWalletId = transaccion.CuentaWalletId,
                Categoria = "Test",
                Divisa = "tes",
                Cuenta = "Test"
            };

            var transacciones = await _contabilidaService.InsertarContabilidadPersonalAsyncService(editContabilidad);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("transacciones/eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarDto eliminar)
        {
            if (eliminar == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }
            var transaccion = await _contabilidaService.EliminarContabilidadPersonalAsyncService(eliminar.Id);
            return transaccion.Success ?
                Ok() :
                BadRequest(transaccion.Message);
        }

    }
}
