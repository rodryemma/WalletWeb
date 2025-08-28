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
    public class TransferenciaService : ITransferenciaService
    {
        ITransferenciaRepository _TransferenciaRepository;

        public TransferenciaService(ITransferenciaRepository transferenciaRepository)
        {
            _TransferenciaRepository = transferenciaRepository;
        }
        public Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaDBFullAsyncService()
        {
            return _TransferenciaRepository.ObtenerTransferenciaDBFullAsync();
        }

        public Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaJoinDBFullAsyncService(DateTime xFechaDesde)
        {
            return _TransferenciaRepository.ObtenerTransferenciaJoinDBFullAsync(xFechaDesde);
        }

        public Task<OperationResult<int>> EditarTransferenciaAsyncService(Transferencia xTransferencia)
        {
            return _TransferenciaRepository.EditarTransferenciaAsync(xTransferencia);
        }

        public Task<OperationResult<int>> EliminarTransferenciaAsyncService(int xId)
        {
            return _TransferenciaRepository.EliminarTransferenciaAsync(xId);
        }

        public Task<OperationResult<int>> InsertarTransferenciaAsyncService(Transferencia xTransferencia)
        {
            return _TransferenciaRepository.InsertarTransferenciaAsync(xTransferencia);
        }

        
    }
}
