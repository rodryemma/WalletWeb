using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransferenciaService
    {
        Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaDBFullAsyncService();
        Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaJoinDBFullAsyncService(DateTime xFechaDesde);
        Task<OperationResult<int>> EditarTransferenciaAsyncService(Transferencia xTransferencia);
        Task<OperationResult<int>> EliminarTransferenciaAsyncService(int xId);
        Task<OperationResult<int>> InsertarTransferenciaAsyncService(Transferencia xTransferencia);
        
    }
}
