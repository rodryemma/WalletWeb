using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetTransacciones(string tipoMovimiento = "Total")
        {
            var transacciones = await _contabilidaService.ObtenerContabilidadDBFullAsyncService(tipoMovimiento.ToLower());
            var lista = transacciones.Select(x => new {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.Categoria,
                x.Cuenta,
                x.MontoUsd,
                x.Comentario,
                x.TipoMovimiento                
            }).ToList();
            return Json(new
            {
                data = transacciones
            });

        }
    }
}
