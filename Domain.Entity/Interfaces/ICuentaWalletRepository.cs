using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface ICuentaWalletRepository
    {
        Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletDBFullAsync();
        Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletJoinDBFullAsync();
        Task<OperationResult<List<CuentaWallet>>> ObtenerMultiplesCuentasAsync(List<int> ids);
        Task<OperationResult<List<CuentaWallet>>> ObtenerMultiplesCuentasAsync(List<string> nombres);
        Task<OperationResult<int>> InsertarCuentaWalletAsync(CuentaWallet xCuentaWallet);
        Task<OperationResult<int>> EditarCuentaWalletAsync(CuentaWallet xCuentaWallet);
        Task<OperationResult<int>> EliminarCuentaWalletAsync(int xId);

    }
}
