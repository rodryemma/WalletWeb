﻿using Domain.Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IContabilidadService
    {
        List<Contabilidad> ObtenerContabilidadDBFullService(string xTipo);
        Task<List<Contabilidad>> ObtenerContabilidadDBFullAsyncService(string xTipo);
        Task<List<Contabilidad>> ObtenerContabilidadDBFullAsyncService(string xTipo, DateTime xFechaDesde);
        DataTable ObtenerContabilidadDBFullService();
        Task<int> InsertarContabilidadPersonalAsyncService(Contabilidad xContabilidad, string xValorCCL);
    }
}
