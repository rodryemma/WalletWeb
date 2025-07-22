using Application.Interfaces;
using Domain.Model.Entity;
using Domain.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ContabilidadService : IContabilidadService
    {
        IContabilidadRepository _ContabilidadRepository;

        public ContabilidadService(IContabilidadRepository xContabilidadRepository)
        {
            _ContabilidadRepository = xContabilidadRepository;
        }

        public List<Contabilidad> ObtenerContabilidadDBFullService(string xTipo)
        {
            return _ContabilidadRepository.ObtenerContabilidadDBFull(xTipo);
        }

        public async Task<List<Contabilidad>> ObtenerContabilidadDBFullAsyncService(string xTipo)
        {
            return await _ContabilidadRepository.ObtenerContabilidadDBFullAsync(xTipo);
        }

        public async Task<List<Contabilidad>> ObtenerContabilidadDBFullAsyncService(string xTipo, DateTime xFechaDesde)
        {
            return await _ContabilidadRepository.ObtenerContabilidadDBFullAsync(xTipo, xFechaDesde);
        }

        public DataTable ObtenerContabilidadDBFullService()
        {
            return _ContabilidadRepository.ObtenerContabilidadDBFull();
        }

        public async Task<int> InsertarContabilidadPersonalAsyncService(Contabilidad xContabilidad, string xValorCCL)
        {
            return await _ContabilidadRepository.InsertarContabilidadPersonalAsync(xContabilidad, xValorCCL);
        }
    }
}
