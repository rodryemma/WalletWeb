using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UI.WalletWeb.Controllers
{
    public class GraficoController : Controller
    {
        IContabilidadService _contabilidaService;
        IReporteService _reporteService;

        public GraficoController(IContabilidadService contabilidadService, IReporteService reporteService)
        {
            _contabilidaService = contabilidadService;
            _reporteService = reporteService;
        }

        [HttpGet]
        public IActionResult Contabilidad()
        {
            return View();
        }

        [HttpGet("reportes/contabilidad")]
        public async Task<IActionResult> GetTransacciones(string tipoMovimiento = "Total", string fecha = "2025-01-01")
        {
            var FechaObtenida = ValidationHelper.ValidarFecha(fecha);
            var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(tipoMovimiento.ToLower(), FechaObtenida);
            if (!transacciones.Success) { return BadRequest(transacciones.Message); }
            
            ChartResultDto result = _reporteService.ObtenerTransaccionesPorMes(transacciones);

            return Json(result);
        }

    }
}
