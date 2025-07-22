using Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IContabilidadRepository
    {
        List<Contabilidad> ObtenerContabilidadDBFull(string xTipo);
        Task<List<Contabilidad>> ObtenerContabilidadDBFullAsync(string xTipo);
        Task<List<Contabilidad>> ObtenerContabilidadDBFullAsync(string xTipo, DateTime xFechaDesde);
        DataTable ObtenerContabilidadDBFull();
        Task<int> InsertarContabilidadPersonalAsync(Contabilidad xContabilidad, string xValorCCL);
    }
}
