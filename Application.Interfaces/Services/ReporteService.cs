using Application.DTOs;
using Application.Interfaces;
using Domain.Model.Enums;
using Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReporteService : IReporteService
    {
        public ChartResultDto ObtenerTransaccionesMontoPorMes(OperationResult<List<ContabilidadDto>> list, ReporteFiltroCategoriaDto reporteFiltroCategoriaDto)
        {
            var data = list.Data;
            var categoriaFiltro = reporteFiltroCategoriaDto.CategoriaFiltro;
            var fechaHasta = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaHasta);
            // . Filtrar según categoriaFiltro (solo si no es null)
            var dataFiltrada = categoriaFiltro == null || categoriaFiltro.Count == 0
                ? data
                : data.Where(x =>
                {
                    var tipoMov = x.TipoMovimiento ?? "Sin tipo";
                    var fechaList = x.Fecha;

                    // Si no existe el tipo en el filtro, no incluir
                    if (!categoriaFiltro.ContainsKey(tipoMov) || fechaList > fechaHasta)
                        return false;

                    var subcategorias = categoriaFiltro[tipoMov];

                    // Si la subcategoría del registro está en true, incluir
                    return subcategorias.ContainsKey(x.Categoria) && subcategorias[x.Categoria];
                }).ToList();



            // . Agrupación base: Año + Mes + TipoMovimiento
            var agrupado = dataFiltrada
                .GroupBy(x => new
                {
                    x.Fecha.Year,
                    x.Fecha.Month,
                    x.TipoMovimiento
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TipoMovimiento = g.Key.TipoMovimiento ?? "Sin tipo",
                    Total = g.Sum(x => x.MontoUsd)
                })
                .ToList();

            // . Meses ordenados
            var meses = agrupado
                .Select(x => new { x.Year, x.Month })
                .Distinct()
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // . Labels de meses
            var labels = meses
                .Select(m => new DateTime(m.Year, m.Month, 1)
                    .ToString("MMM yyyy", CultureInfo.GetCultureInfo("es-AR")))
                .ToList();

            //Le damos un orden a cual mostrar primero en las barras
            var ordenTipoMovimiento = new Dictionary<string, int>
            {
                { ContabilidadTipoEnums.Ingresos.ToString().ToLower(), 1 },
                { ContabilidadTipoEnums.Gastos.ToString().ToLower(), 2 }
            };

            // . Series por tipo de movimiento
            var series = agrupado
                .Select(x => x.TipoMovimiento)
                .Distinct()
                .OrderBy(tipo =>
                    ordenTipoMovimiento.ContainsKey(tipo)
                        ? ordenTipoMovimiento[tipo]
                        : int.MaxValue)
                .Select(tipo => new ChartSerieDto
                {
                    Label = tipo,
                    Data = meses.Select(m =>
                        agrupado
                            .Where(x =>
                                x.TipoMovimiento == tipo &&
                                x.Year == m.Year &&
                                x.Month == m.Month)
                            .Sum(x => x.Total)
                    ).ToList()
                })
                .ToList();

            return new ChartResultDto
            {
                Labels = labels,
                Series = series
            };
        }

        public ChartResultDto ObtenerTransaccionesMontoPorCategoria(OperationResult<List<ContabilidadDto>> list, ReporteFiltroCategoriaDto reporteFiltroCategoriaDto)
        {
            var data = list.Data;
            var categoriaFiltro = reporteFiltroCategoriaDto.CategoriaFiltro;
            var fechaHasta = ValidationHelper.ValidarFecha(reporteFiltroCategoriaDto.FechaHasta);
            // . Filtrar según categoriaFiltro (solo si no es null)
            var dataFiltrada = categoriaFiltro == null || categoriaFiltro.Count == 0
                ? data
                : data.Where(x =>
                {
                    var tipoMov = x.TipoMovimiento ?? "Sin tipo";
                    var fechaList = x.Fecha;

                    // Si no existe el tipo en el filtro, no incluir
                    if (!categoriaFiltro.ContainsKey(tipoMov) || fechaList > fechaHasta)
                        return false;

                    var subcategorias = categoriaFiltro[tipoMov];

                    // Si la subcategoría del registro está en true, incluir
                    return subcategorias.ContainsKey(x.Categoria) && subcategorias[x.Categoria];
                }).ToList();

            var agrupado = dataFiltrada
                .GroupBy(x => new
                {
                    x.Categoria
                })
                .Select(g => new
                {
                    Categoria = g.Key.Categoria ?? "Sin tipo",
                    Total = Math.Round(g.Sum(x => x.MontoUsd), 2)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var labels = agrupado
                .Select(m => m.Categoria)
                .ToList();

            var serieData = agrupado
                .Select(m => m.Total)
                .ToList();

            var series = new List<ChartSerieDto>();

            series.Add(new ChartSerieDto
                {
                    Data = serieData
                }
            );

            return new ChartResultDto
            {
                Labels = labels,
                Series = series
            };
        }
    }
}
