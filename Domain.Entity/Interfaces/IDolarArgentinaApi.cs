using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IDolarArgentinaApi
    {
        Task<OperationResult<string>> obtenerCCL();
    }
}
