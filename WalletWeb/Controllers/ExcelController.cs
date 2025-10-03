using Application.Interfaces;
using ClosedXML.Excel;
using Domain.Model.Entites;
using Domain.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UI.WalletWeb.Controllers
{
    public class ExcelController : Controller
    {
        IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("excel/import")]
        public async Task<IActionResult> ImportExcel(IFormFile excelFile)
        {
            try
            {
                if (excelFile == null || excelFile.Length == 0)
                {
                    return Json(new { success = false, message = "No se seleccionó archivo" });
                }

                // Validar extensión
                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var fileExtension = Path.GetExtension(excelFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Json(new { success = false, message = "Formato de archivo no válido" });
                }

                var responseGastos = new OperationResult<List<ExcelContabilidad>>();
                var responseIngresos = new OperationResult<List<ExcelContabilidad>>();
                var responseTransacciones = new OperationResult<List<ExcelContabilidad>>();


                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    stream.Position = 0;

                    responseGastos = await _excelService.LeerExcelContabilidad(stream, "gastos");
                    responseIngresos = await _excelService.LeerExcelContabilidad(stream, "ingresos");
                }

                if (responseGastos.Success)
                {
                    var rta = await _excelService.GuardarExcelContabilidad(responseGastos.Data, ContabilidadTipoEnums.Gastos.ToString());
                }

                if (responseIngresos.Success)
                {
                    var rta = await _excelService.GuardarExcelContabilidad(responseIngresos.Data, ContabilidadTipoEnums.Ingresos.ToString());
                }

                if (responseTransacciones.Success)
                {

                }

                // Aquí procesarías los datos según tu lógica de negocio
                // Por ejemplo: guardar en base de datos, validar, etc.

                return Json(new
                {
                    success = responseGastos.Success,
                    message = $"Se procesaron {responseGastos.Data.Count} registros correctamente",
                    data = responseGastos.Data.Take(5) // Muestra solo los primeros 5 para preview
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


    }
}
