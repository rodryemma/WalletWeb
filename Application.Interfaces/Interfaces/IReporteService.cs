using Application.DTOs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReporteService
    {
        ChartResultDto ObtenerTransaccionesMontoPorMes(OperationResult<List<ContabilidadDto>> list, ReporteFiltroCategoriaDto reporteFiltroCategoriaDto);
        ChartResultDto ObtenerTransaccionesMontoPorCategoria(OperationResult<List<ContabilidadDto>> list, ReporteFiltroCategoriaDto reporteFiltroCategoriaDto);

    }
}
