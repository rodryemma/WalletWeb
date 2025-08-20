using Application.DTOs;
using Application.Interfaces;
using Domain.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.CodeDom;
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
            //var transacciones = await _contabilidaService.ObtenerContabilidadDBFullAsyncService(tipoMovimiento.ToLower());
            var transacciones = await _contabilidaService.ObtenerContabilidadDBFullAsyncService(tipoMovimiento.ToLower(), FechaObtenida);
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
                x.MontoUsd
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
                Categoria = transaccion.Categoria,
                Comentario = transaccion.Comentario,
                Cuenta = transaccion.Cuenta,
                Divisa = transaccion.Divisa,
                TipoMovimiento = transaccion.TipoMovimiento,
                ValorCCL = transaccion.ValorCCL
            };

            var transacciones = await _contabilidaService.EditarContabilidadPersonalAsyncService(editContabilidad);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("transacciones/crear")]
        public async Task<IActionResult> Insertar([FromBody] ContabilidadDto transaccion)
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
                Fecha = transaccion.Fecha,
                CantidadDivisa = transaccion.CantidadDivisa,
                Categoria = transaccion.Categoria,
                Comentario = transaccion.Comentario,
                Cuenta = transaccion.Cuenta,
                Divisa = transaccion.Divisa,
                TipoMovimiento = transaccion.TipoMovimiento,
                ValorCCL = transaccion.ValorCCL
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
