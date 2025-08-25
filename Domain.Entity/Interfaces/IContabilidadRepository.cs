using Domain.Model.Entity;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IContabilidadRepository
    {
        List<Contabilidad> ObtenerContabilidadDBFull(string xTipo);
        Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadDBFullAsync(string xTipo);
        Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadDBFullAsync(string xTipo, DateTime xFechaDesde);
        Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadJoinDBFullAsync(string xTipo, DateTime xFechaDesde);
        DataTable ObtenerContabilidadDBFull();
        Task<int> InsertarContabilidadPersonalAsync(Contabilidad xContabilidad, string xValorCCL);
        Task<OperationResult<int>> EditarContabilidadPersonalAsync(Contabilidad xContabilidad);
        Task<OperationResult<int>> InsertarContabilidadPersonalAsync(Contabilidad xContabilidad);
        Task<OperationResult<int>> EliminarContabilidadPersonalAsync(int xId);
    }
}
