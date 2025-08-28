using Domain.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI.WalletWeb.Controllers
{
    public class DolarController : Controller
    {
        private readonly IDolarArgentinaApi _dolarArgentinaApi;
        private readonly IAmbitoApi _ambitoApi;
        private readonly IDolarService _dolarService;

        public DolarController(IDolarService dolarService, IDolarArgentinaApi dolarArgentinaApi, IAmbitoApi ambitoApi)
        {
            _dolarService = dolarService;
            _dolarArgentinaApi = dolarArgentinaApi;
            _ambitoApi = ambitoApi;
        }

        [HttpGet("ccl/json")]
        public async Task<IActionResult> Actualizar()
        {
            await _ambitoApi.ActualizarCotizacionAsync();

            return Json(_dolarService.DolarCCL);
        }

        [HttpGet("cclhistorico/json")]
        public async Task<IActionResult> ObtenerHistorico(DateTime fechaInicio, DateTime fechaFinal)
        {
            var rta = await _ambitoApi.ObtenerHistoricoCCL(fechaInicio, fechaFinal);
            if (!rta.Success) { return BadRequest(rta.Message); }

            return Json(rta.Data);
        }
    }
}
