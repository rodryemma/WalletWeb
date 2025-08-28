using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface ITransferenciaRepository
    {
        Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaDBFullAsync();
        Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaJoinDBFullAsync(DateTime xFechaDesde);
        Task<OperationResult<int>> InsertarTransferenciaAsync(Transferencia xTransferencia);
        Task<OperationResult<int>> EditarTransferenciaAsync(Transferencia xTransferencia);
        Task<OperationResult<int>> EliminarTransferenciaAsync(int xId);
    }
}
