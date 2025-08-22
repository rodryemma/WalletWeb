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
    public class CuentaWalletService : ICuentaWalletService
    {
        ICuentaWalletRepository _cuentaWalletRepository;

        public CuentaWalletService(ICuentaWalletRepository cuentaWalletRepository)
        {
            _cuentaWalletRepository = cuentaWalletRepository;
        }

        public Task<OperationResult<int>> EditarCuentaWalletAsyncService(CuentaWallet xCuentaWallet)
        {
            return _cuentaWalletRepository.EditarCuentaWalletAsync(xCuentaWallet);
        }

        public Task<OperationResult<int>> EliminarCuentaWalletAsyncService(int xId)
        {
            return _cuentaWalletRepository.EliminarCuentaWalletAsync(xId);
        }

        public Task<OperationResult<int>> InsertarCuentaWalletAsyncService(CuentaWallet xCuentaWallet)
        {
            return _cuentaWalletRepository.InsertarCuentaWalletAsync(xCuentaWallet);
        }

        public Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletDBFullAsyncService()
        {
            return _cuentaWalletRepository.ObtenerCuentaWalletDBFullAsync();
        }
    }
}
