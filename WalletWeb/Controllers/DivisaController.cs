using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Model.Entites;
using Microsoft.AspNetCore.Mvc;

namespace UI.WalletWeb.Controllers
{
    public class DivisaController : Controller
    {
        IDivisaService _divisaService;

        public DivisaController(IDivisaService divisaService)
        {
            _divisaService = divisaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("divisa/json")]
        public async Task<IActionResult> GetDivisa()
        {
            var divisa = await _divisaService.ObtenerDivisaDBFullAsyncService();
            if (!divisa.Success) { return BadRequest(divisa.Message); }

            var lista = divisa.Data.Select(x => new
            {
                x.Id,
                x.Nombre,
                x.Descripcion
            }).ToList();

            return Json(new
            {
                data = lista
            });

        }

        [HttpPost("divisa/crear")]
        public async Task<IActionResult> Insertar([FromBody] DivisaDto divisa)
        {
            //TODO : Validar que no exista el nombre
            if (divisa == null)
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
            var editDivisa = new Divisa()
            {
                Nombre = divisa.Nombre,
                Descripcion = divisa.Descripcion
            };

            var transacciones = await _divisaService.InsertarDivisaAsyncService(editDivisa);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("divisa/eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarDto eliminar)
        {
            if (eliminar == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }
            var divisa = await _divisaService.EliminarDivisaAsyncService(eliminar.Id);
            return divisa.Success ?
                Ok() :
                BadRequest(divisa.Message);
        }

        [HttpPost("divisa/editar")]
        public async Task<IActionResult> Editar([FromBody] DivisaUpdateDto divisa)
        {
            if (divisa == null)
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
            var editDivisa = new Divisa()
            {
                Id = divisa.Id,
                Nombre = divisa.Nombre,
                Descripcion = divisa.Descripcion
            };

            var transacciones = await _divisaService.EditarDivisaAsyncService(editDivisa);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }
    }
}
