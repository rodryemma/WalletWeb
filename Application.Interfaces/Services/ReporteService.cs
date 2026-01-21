using Application.DTOs;
using Application.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReporteService : IReporteService
    {
        public ChartResultDto ObtenerTransaccionesMontoPorMes(OperationResult<List<ContabilidadDto>> list, Dictionary<string, Dictionary<string, bool>> categoriaFiltro)
        {
            var data = list.Data;

            // . Filtrar según categoriaFiltro (solo si no es null)
            var dataFiltrada = categoriaFiltro == null || categoriaFiltro.Count == 0
                ? data
                : data.Where(x => {
                    var tipoMov = x.TipoMovimiento ?? "Sin tipo";

                    // Si no existe el tipo en el filtro, no incluir
                    if (!categoriaFiltro.ContainsKey(tipoMov))
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
                { "ingresos", 1 },
                { "gastos", 2 }
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
    }
}
