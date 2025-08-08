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

            var valor = _dolarService.DolarCCL;

            return Json(new
            {
                ccl = valor
            });
        }
    }
}
