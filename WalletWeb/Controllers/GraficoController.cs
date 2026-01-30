using Application.DTOs;
using Application.Interfaces;
using DocumentFormat.OpenXml.Vml;
using Domain.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Extensions;

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
        public IActionResult ContabilidadBar()
        {
            return View();
        }

        [HttpPost("Reportes/Contabilidad/Barra")]        
        public async Task<IActionResult> GetTransacciones([FromBody] ReporteFiltroCategoriaDto reporteFiltroCategoriaDto)
        {
            if (reporteFiltroCategoriaDto == null)
            {
                return BadRequest("El reporte de filtro de categoria enviado es nula.");
            }

            var FechaDesde = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaDesde);
            var FechaHasta = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaHasta);
            var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(reporteFiltroCategoriaDto.TipoMovimiento.ToLower(), FechaDesde, FechaHasta);

            //var FechaObtenida = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaDesde);
            //var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(reporteFiltroCategoriaDto.TipoMovimiento.ToLower(), FechaObtenida);
            if (!transacciones.Success) { return BadRequest(transacciones.Message); }

            var categoria = await _categoriaService.ObtenerCategoriaDBFullAsyncService(ContabilidadTipoEnums.Total.ToLowerString());

            Dictionary<string, Dictionary<string, bool>> diccionarioCategoria;

            if (reporteFiltroCategoriaDto.CategoriaFiltro == null || reporteFiltroCategoriaDto.CategoriaFiltro.Count == 0)
            {
                diccionarioCategoria = _categoriaService.ArmarDiccionarioCategoriaService(categoria);
            }
            else
            {
                diccionarioCategoria = reporteFiltroCategoriaDto.CategoriaFiltro;
            }

            ChartResultDto result = _reporteService.ObtenerTransaccionesMontoPorMes(transacciones, reporteFiltroCategoriaDto);
            ReporteCategoriaDto response = new ReporteCategoriaDto
            {
                Chart = result,
                DiccionarioCategoria = diccionarioCategoria
            };
            return Json(response);
        }

        [HttpGet("Contabilidad/Grafico/Torta")]
        public IActionResult ContabilidadPie()
        {
            return View();
        }

        [HttpPost("Reportes/Contabilidad/Torta")]
        public async Task<IActionResult> GetTransaccionesPie([FromBody] ReporteFiltroCategoriaDto reporteFiltroCategoriaDto)
        {
            if (reporteFiltroCategoriaDto == null)
            {
                return BadRequest("El reporte de filtro de categoria enviado es nula.");
            }

            var FechaDesde = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaDesde);
            var FechaHasta = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaHasta);

            var transacciones = await _contabilidaService.ObtenerContabilidadJoinDBFullAsyncService(reporteFiltroCategoriaDto.TipoMovimiento.ToLower(), FechaDesde, FechaHasta);
            if (!transacciones.Success) { return BadRequest(transacciones.Message); }

            ChartResultDto result = _reporteService.ObtenerTransaccionesMontoPorCategoria(transacciones, reporteFiltroCategoriaDto);
            ReporteCategoriaDto response = new ReporteCategoriaDto
            {
                Chart = result
            };
            return Json(response);
        }

    }
}
