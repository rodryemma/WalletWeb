using Application.DTOs;
using Domain.Model.Entites;
using Domain.Model.Entity;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExcelService
    {
        Task<OperationResult<List<ExcelContabilidad>>> LeerExcelContabilidad(Stream stream, string tipo);
        Task<OperationResult<List<Contabilidad>>> MapearExcelAContabilidad(List<ExcelContabilidad> listExcel, string tipo);
        Task<OperationResult<int>> GuardarExcelContabilidad(List<ExcelContabilidad> listExcel, string tipo);
    }
}
