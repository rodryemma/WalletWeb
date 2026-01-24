using Application.DTOs;
using Application.Interfaces;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UI.WalletWeb.Controllers
{
    public class GraficoController : Controller
    {
        IContabilidadService _contabilidaService;
        IReporteService _reporteService;
        ICategoriaService _categoriaService;

        public GraficoController(IContabilidadService contabilidadService, IReporteService reporteService, ICategoriaService categoriaService)
        {
            _contabilidaService = contabilidadService;
            _reporteService = reporteService;
            _categoriaService = categoriaService;
        }

        [HttpGet("Contabilidad/Grafico/Barra")]
        public IActionResult Contabilidad()
        {
            return View();
        }

        //[HttpGet("reportes/contabilidad/barra
        [HttpPost("reportes/contabilidad/barra")]        
        public async Task<IActionResult> GetTransacciones([FromBody] ReporteFiltroCategoriaDto reporteFiltroCategoriaDto)
        {
            if (reporteFiltroCategoriaDto == null)
            {
                return BadRequest("El reporte de filtro de categoria enviado es nula.");
            }

            var FechaObtenida = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.Fecha);
            var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(reporteFiltroCategoriaDto.TipoMovimiento.ToLower(), FechaObtenida);
            if (!transacciones.Success) { return BadRequest(transacciones.Message); }

            var categoria = await _categoriaService.ObtenerCategoriaDBFullAsyncService("total");

            Dictionary<string, Dictionary<string, bool>> diccionarioCategoria;

            if (reporteFiltroCategoriaDto.CategoriaFiltro == null || reporteFiltroCategoriaDto.CategoriaFiltro.Count == 0)
            {
                diccionarioCategoria = _categoriaService.ArmarDiccionarioCategoriaService(categoria);
            }
            else
            {
                diccionarioCategoria = reporteFiltroCategoriaDto.CategoriaFiltro;
            }

            ChartResultDto result = _reporteService.ObtenerTransaccionesMontoPorMes(transacciones, reporteFiltroCategoriaDto.CategoriaFiltro);
            ReporteCategoriaDto response = new ReporteCategoriaDto
            {
                Chart = result,
                DiccionarioCategoria = diccionarioCategoria
            };
            return Json(response);
        }

    }
}
