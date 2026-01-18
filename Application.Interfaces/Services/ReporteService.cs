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
        public ChartResultDto ObtenerTransaccionesPorMes(OperationResult<List<ContabilidadDto>> list)
        {
            var data = list.Data;

            // 1. Agrupación base: Año + Mes + TipoMovimiento
            var agrupado = data
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

            // 2. Meses ordenados
            var meses = agrupado
                .Select(x => new { x.Year, x.Month })
                .Distinct()
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // 3. Labels de meses
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

            // 4. Series por tipo de movimiento
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
