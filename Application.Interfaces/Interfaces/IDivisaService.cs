using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDivisaService
    {
        Task<OperationResult<int>> EditarDivisaAsyncService(Divisa xDivisa);
        Task<OperationResult<int>> EliminarDivisaAsyncService(int xId);
        Task<OperationResult<int>> InsertarDivisaAsyncService(Divisa xDivisa);
        Task<OperationResult<List<Divisa>>> ObtenerDivisaDBFullAsyncService();
        Task<OperationResult<List<Divisa>>> ObtenerMultiplesDivisasAsyncService(List<int> xIds);
        Task<Dictionary<int, string>> ObtenerDivisasDictionarioAsync<T>(IEnumerable<T> entidades, Func<T, int> divisaIdSelector);
    }
}
