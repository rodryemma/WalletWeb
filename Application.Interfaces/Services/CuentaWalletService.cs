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

        public Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletDBFullAsyncService()
        {
            return _cuentaWalletRepository.ObtenerCuentaWalletDBFullAsync();
        }

        public Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletJoinDBFullAsyncService()
        {
            return _cuentaWalletRepository.ObtenerCuentaWalletJoinDBFullAsync();
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

        public Task<OperationResult<List<CuentaWallet>>> ObtenerMultiplesCuentasAsyncService(List<int> ids)
        {
            return _cuentaWalletRepository.ObtenerMultiplesCuentasAsync(ids);
        }

        public Task<OperationResult<List<CuentaWallet>>> ObtenerMultiplesCuentasAsyncService(List<string> nombres)
        {
            return _cuentaWalletRepository.ObtenerMultiplesCuentasAsync(nombres);
        }

        public async Task<Dictionary<int, string>> ObtenerCuentasDictionarioAsync<T>(IEnumerable<T> entidades, Func<T, int> divisaIdSelector)
        {
            var divisaIds = entidades.Select(divisaIdSelector).Distinct().ToList();

            if (!divisaIds.Any())
                return new Dictionary<int, string>();

            var divisasResult = await ObtenerMultiplesCuentasAsyncService(divisaIds);

            return divisasResult?.Success == true && divisasResult.Data != null
                ? divisasResult.Data.ToDictionary(d => d.Id, d => d.Nombre)
                : new Dictionary<int, string>();
        }
    }
}
