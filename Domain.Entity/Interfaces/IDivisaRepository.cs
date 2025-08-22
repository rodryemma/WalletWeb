using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IDivisaRepository
    {
        Task<OperationResult<List<Divisa>>> ObtenerDivisaDBFullAsync();
        Task<OperationResult<List<Divisa>>> ObtenerMultiplesDivisasAsync(List<int> xIds);
        Task<OperationResult<int>> InsertarDivisaAsync(Divisa xDivisa);
        Task<OperationResult<int>> EditarDivisaAsync(Divisa xDivisa);
        Task<OperationResult<int>> EliminarDivisaAsync(int xId);
        
    }
}
