using Application.DTOs;
using Application.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Model.Entites;
using Domain.Model.Entity;
using Domain.Model.Enums;
using Domain.Model.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class ExcelService : IExcelService
    {
        IContabilidadService _contabilidadService;
        ICategoriaService _categoriaService;
        ICuentaWalletService _cuentaWalletService;
        IDivisaService _divisaService;
        IAmbitoApi _ambitoApi;

        public ExcelService(ICategoriaService categoriaService, ICuentaWalletService cuentaWalletService, IDivisaService divisaService, IAmbitoApi ambitoApi, IContabilidadService contabilidadService)
        {
            _categoriaService = categoriaService;
            _cuentaWalletService = cuentaWalletService;
            _divisaService = divisaService;
            _ambitoApi = ambitoApi;
            _contabilidadService = contabilidadService;
        }

        public async Task<OperationResult<List<ExcelContabilidad>>> LeerExcelContabilidad(Stream stream, string tipo)
        {
            var dataExcel = new List<ExcelContabilidad>();

            using (var workbook = new XLWorkbook(stream))
            {
                int indexHoja = 0;

                for(int i = 1; i<= 3 ; i++) 
                {
                    var worksheetName = workbook.Worksheet(i).Name;
                    if (workbook.Worksheet(i).Name.ToLower() == tipo.ToLower())
                        indexHoja = i;
                }
                var worksheet = workbook.Worksheet(indexHoja);
                var range = worksheet.RangeUsed().RowsUsed();

                if (range == null)
                {
                    OperationResult<List<ExcelContabilidad>>.Fail("El archivo esta vacio");
                }

                try
                {

                    foreach (var row in range.Skip(2)) // saltamos cabecera
                    {
                        var excel = new ExcelContabilidad
                        {
                            Fecha = row.Cell(1).GetValue<DateTime>(),
                            Categoria = row.Cell(2).GetValue<string>(),
                            Cuenta = row.Cell(3).GetValue<string>(),
                            CantidadDivisa = row.Cell(4).GetValue<double>(),
                            Divisa = row.Cell(5).GetValue<string>(),
                            Comentario = row.Cell(11).GetValue<string>()
                        };

                        dataExcel.Add(excel);
                    }
                }
                catch (Exception ex)
                {
                    OperationResult<List<ExcelContabilidad>>.Fail($"La esctructura de la hoja '{tipo}' es incorrecto");
                }
            }

            return OperationResult<List<ExcelContabilidad>>.Ok(dataExcel);
        }

        public async Task<OperationResult<List<Contabilidad>>> MapearExcelAContabilidad(List<ExcelContabilidad> listExcel, string tipo)
        {
            if(listExcel == null && listExcel.Count == 0)
                OperationResult<List<ExcelContabilidad>>.Fail("El archivo esta vacio");

            try
            {
                //TODO : Cambiar en la busqueda por nombre que este el tipo de movimiento
                var nombreCategoria = listExcel.Select(x => x.Categoria.ToLower()).Distinct().ToList();
                var nombreCuentas = listExcel.Select(x => x.Cuenta.ToLower()).Distinct().ToList();
                var nombreDivisa = listExcel.Select(x => x.Divisa.ToLower()).Distinct().ToList();
                var ListFechas = listExcel.Select(x => x.Fecha).Distinct().ToList();

                var listCategoria = await _categoriaService.ObtenerMultiplesCategoriasAsyncService(nombreCategoria, tipo.ToLower());
                var listCuentas = await _cuentaWalletService.ObtenerMultiplesCuentasAsyncService(nombreCuentas);
                var listDivisas = await _divisaService.ObtenerMultiplesDivisasAsyncService(nombreDivisa);

                var diccCategoria = listCategoria.Data.DistinctBy(c => c.Nombre).ToDictionary(c => c.Nombre.ToLower(), c => c.Id);
                var diccCuentas = listCuentas.Data.DistinctBy(c => c.Nombre).ToDictionary(c => c.Nombre.ToLower(), c => c.Id);
                var diccDivisas = listDivisas.Data.DistinctBy(c => c.Nombre).ToDictionary(c => c.Nombre.ToLower(), c => c.Id);
                var arrayFechas = await Task.WhenAll(ListFechas.Select(async fecha => new
                {
                    Fecha = fecha,
                    ValorCCL = await _ambitoApi.ObtenerHistoricoCCL(fecha.AddDays(-7), fecha)
                }));

                var diccFechas = arrayFechas.ToDictionary(x => x.Fecha, x => double.Parse(x.ValorCCL.Data.ToString()));

                List<Contabilidad> mapListExcel = listExcel.Select(x => new Contabilidad
                {
                    Fecha = x.Fecha,
                    CantidadDivisa = x.CantidadDivisa,
                    Comentario = x.Comentario,
                    TipoMovimiento = tipo,
                    ValorCCL = (diccFechas.ContainsKey(x.Fecha) ? diccFechas[x.Fecha] : 0),
                    CategoriaId = (diccCategoria.ContainsKey(x.Categoria.ToLower()) ? diccCategoria[x.Categoria.ToLower()] : 30),
                    DivisaId = (diccCuentas.ContainsKey(x.Divisa.ToLower()) ? diccCuentas[x.Divisa.ToLower()] : 2),
                    CuentaWalletId = (diccCuentas.ContainsKey(x.Cuenta.ToLower()) ? diccCuentas[x.Cuenta.ToLower()] : 5)
                }).ToList();

                return OperationResult<List<Contabilidad>>.Ok(mapListExcel);
            }
            catch (Exception ex)
            {
                return OperationResult<List<Contabilidad>>.Fail("Error en mapear Excel a contabilidad : " + ex.Message);
            }
        }

        public async Task<OperationResult<int>> GuardarExcelContabilidad(List<ExcelContabilidad> listExcel, string tipo)
        {
            if((listExcel?.Count ?? 0) == 0)
            {
                return OperationResult<int>.Ok(0, $"Sin elementos en: {tipo}");
            }

            if (ValidationHelper.CompararEnum(tipo, ContabilidadTipoEnums.Gastos) || ValidationHelper.CompararEnum(tipo, ContabilidadTipoEnums.Ingresos))
            {
                var mapContabilidad = await MapearExcelAContabilidad(listExcel, tipo);
                if (mapContabilidad.Success)
                {
                    try
                    {
                        var rta = await _contabilidadService.InsertarMultipleContabilidadPersonalAsyncService(mapContabilidad.Data);
                        return OperationResult<int>.Ok(rta.Data);
                    }
                    catch (Exception ex)
                    {
                        return OperationResult<int>.Fail(ex.Message);
                    }
                }
                else
                {
                    return OperationResult<int>.Fail(mapContabilidad.Message);
                }
            }

            return OperationResult<int>.Fail("No corresponde el tipo de movimiento");
        }
    }
}
