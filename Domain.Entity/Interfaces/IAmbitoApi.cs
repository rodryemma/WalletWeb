using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IAmbitoApi
    {
        Task<OperationResult<string>> ObtenerHistoricoCCL(DateTime fechaInicio, DateTime fechaFinal);
        Task<OperationResult<string>> obtenerCCLAmbito();
        Task ActualizarCotizacionAsync();
    }
}
