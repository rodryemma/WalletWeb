using Application.DTOs;
using Domain.Model.Entity;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IContabilidadService
    {
        List<Contabilidad> ObtenerContabilidadDBFullService(string xTipo);
        Task<OperationResult<List<ContabilidadDto>>> ObtenerContabilidadDBFullAsyncService(string xTipo);
        Task<OperationResult<List<ContabilidadDto>>> ObtenerContabilidadDBFullAsyncService(string xTipo, DateTime xFechaDesde);
        Task<OperationResult<List<ContabilidadDto>>> ObtenerContabilidadJoinDBFullAsyncService(string xTipo, DateTime xFechaDesde);
        Task<OperationResult<List<ContabilidadDto>>> ObtenerContabilidadJoinDBFullAsyncService(string xTipo, DateTime xFechaDesde, DateTime xFechaHasta);
        DataTable ObtenerContabilidadDBFullService();
        Task<int> InsertarContabilidadPersonalAsyncService(Contabilidad xContabilidad, string xValorCCL);
        Task<OperationResult<int>> EditarContabilidadPersonalAsyncService(Contabilidad xContabilidad);
        Task<OperationResult<int>> InsertarContabilidadPersonalAsyncService(Contabilidad xContabilidad);
        Task<OperationResult<int>> InsertarMultipleContabilidadPersonalAsyncService(List<Contabilidad> xContabilidad);
        Task<OperationResult<int>> EliminarContabilidadPersonalAsyncService(int xId);
    }
}
