using Application.Interfaces;
using Domain.Model.Entites;
using Domain.Model.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DivisaService : IDivisaService
    {
        IDivisaRepository _DivisaRepository;

        public DivisaService(IDivisaRepository divisaRepository)
        {
            _DivisaRepository = divisaRepository;
        }

        public Task<OperationResult<int>> EditarDivisaAsyncService(Divisa xDivisa)
        {
            return _DivisaRepository.EditarDivisaAsync(xDivisa);
        }
        public Task<OperationResult<int>> EliminarDivisaAsyncService(int xId)
        {
            return _DivisaRepository.EliminarDivisaAsync(xId);
        }

        public Task<OperationResult<int>> InsertarDivisaAsyncService(Divisa xDivisa)
        {
            return _DivisaRepository.InsertarDivisaAsync(xDivisa);
        }

        public Task<OperationResult<List<Divisa>>> ObtenerDivisaDBFullAsyncService()
        {
            return _DivisaRepository.ObtenerDivisaDBFullAsync();
        }

        public Task<OperationResult<List<Divisa>>> ObtenerMultiplesDivisasAsyncService(List<int> xIds)
        {
            return _DivisaRepository.ObtenerMultiplesDivisasAsync(xIds);
        }
    }
}
