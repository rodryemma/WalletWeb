using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Model.Entites;
using Domain.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UI.WalletWeb.Controllers
{
    public class CuentaWalletController : Controller
    {
        ICuentaWalletService _cuentaWalletService;
        IDivisaService _divisaService;

        public CuentaWalletController(ICuentaWalletService cuentaWalletService, IDivisaService divisaService)
        {
            _cuentaWalletService = cuentaWalletService;
            _divisaService = divisaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("cuentawallet/json")]
        public async Task<IActionResult> GetCuentaWallet()
        {
            var cuentas = await _cuentaWalletService.ObtenerCuentaWalletDBFullAsyncService();
            if (!cuentas.Success) { return BadRequest(cuentas.Message); }

            //Buscar Id Divisa y volcarlos en un diccionario
            var divisaIds = cuentas.Data.Select(x => x.DivisaId).Distinct().ToList();
            var divisasResult = await _divisaService.ObtenerMultiplesDivisasAsyncService(divisaIds);
            var divisasDict = divisasResult?.Success == true && divisasResult.Data != null
                ? divisasResult.Data.ToDictionary(d => d.Id, d => d.Nombre)
                : new Dictionary<int, string>();

            var lista = cuentas.Data.Select(x => new
            {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.Nombre,
                x.Descripcion,
                DivisaId = divisasDict.TryGetValue(x.DivisaId, out string nombre) ? nombre : "Sin divisa"
            }).ToList();

            return Json(new
            {
                data = lista
            });

        }

        [HttpPost("cuentawallet/editar")]
        public async Task<IActionResult> Editar([FromBody] CuentaWalletUpdateDto cuenta)
        {
            if (cuenta == null)
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
            var editCuenta = new CuentaWallet()
            {
                Id = cuenta.Id,
                Fecha = cuenta.Fecha,
                Nombre = cuenta.Nombre,
                Descripcion = cuenta.Descripcion,
                DivisaId = cuenta.DivisaId
            };

            var cuentas = await _cuentaWalletService.EditarCuentaWalletAsyncService(editCuenta);
            return cuentas.Success ?
                Ok() :
                BadRequest(cuentas.Message);
        }

        [HttpPost("cuentawallet/crear")]
        public async Task<IActionResult> Insertar([FromBody] CuentaWalletDto cuenta)
        {
            //TODO : Validar que no exista el nombre
            if (cuenta == null)
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
            var editCuenta = new CuentaWallet()
            {
                Fecha = cuenta.Fecha,
                Nombre = cuenta.Nombre,
                Descripcion = cuenta.Descripcion,
                DivisaId = cuenta.DivisaId
            };

            var cuentas = await _cuentaWalletService.InsertarCuentaWalletAsyncService(editCuenta);
            return cuentas.Success ?
                Ok() :
                BadRequest(cuentas.Message);
        }

        [HttpPost("cuentawallet/eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarDto eliminar)
        {
            if (eliminar == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }
            var transaccion = await _cuentaWalletService.EliminarCuentaWalletAsyncService(eliminar.Id);
            return transaccion.Success ?
                Ok() :
                BadRequest(transaccion.Message);
        }
    }
}
