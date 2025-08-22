using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICuentaWalletService
    {
        Task<OperationResult<int>> EditarCuentaWalletAsyncService(CuentaWallet xCuentaWallet);
        Task<OperationResult<int>> EliminarCuentaWalletAsyncService(int xId);
        Task<OperationResult<int>> InsertarCuentaWalletAsyncService(CuentaWallet xCuentaWallet);
        Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletDBFullAsyncService();

    }
}
