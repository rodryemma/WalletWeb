using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UI.WalletWeb.Controllers
{
    public class CategoriaController : Controller
    {
        ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService xCategoriaService)
        {
            _categoriaService = xCategoriaService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("categoria/json")]
        public async Task<IActionResult> GetCategorias(string tipoMovimiento = "Total")
        {
            var categorias = await _categoriaService.ObtenerCategoriaDBFullAsyncService(tipoMovimiento.ToLower());
            if (!categorias.Success) { return BadRequest(categorias.Message); }

            var lista = categorias.Data.Select(x => new
            {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.Nombre,
                x.Tipo,
                x.Descripcion
            }).ToList();
            return Json(new
            {
                data = lista
            });

        }
    }
}
