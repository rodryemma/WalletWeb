using Application.Common;
using Application.DTOs;
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

        public async Task<List<ContabilidadDto>> ObtenerContabilidadDBFullAsyncService(string xTipo)
        {
            var contabilidadMovimientos = await _ContabilidadRepository.ObtenerContabilidadDBFullAsync(xTipo);
            return contabilidadMovimientos.Select(m => new ContabilidadDto
            {
                Id = m.Id,
                Fecha = m.Fecha,
                Categoria = m.Categoria,
                Cuenta = m.Cuenta,
                CantidadDivisa = m.CantidadDivisa,
                Divisa = m.Divisa,
                Comentario = m.Comentario,
                TipoMovimiento = m.TipoMovimiento,
                ValorCCL = m.ValorCCL,
                MontoUsd = Math.Round(MathHelper.Dividir(m.CantidadDivisa, m.ValorCCL), 2)

            }).ToList();
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

        public async Task<int> EditarContabilidadPersonalAsyncService(Contabilidad xContabilidad)
        {
            return await _ContabilidadRepository.EditarContabilidadPersonalAsync(xContabilidad);
        }

        public async Task<int> InsertarContabilidadPersonalAsyncService(Contabilidad xContabilidad)
        {
            return await _ContabilidadRepository.InsertarContabilidadPersonalAsync(xContabilidad);
        }

        public async Task<int> EliminarContabilidadPersonalAsyncService(int xId)
        {
            return await _ContabilidadRepository.EliminarContabilidadPersonalAsync(xId);
        }
    }
}
