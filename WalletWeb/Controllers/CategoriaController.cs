using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Model.Entites;
using Domain.Model.Entity;
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

            return Json(lista);

        }

        [HttpPost("categoria/ids")]
        public async Task<IActionResult> GetListIds([FromBody] List<int> ids)
        {
            var categorias = await _categoriaService.ObtenerMultiplesCategoriasAsyncService(ids);
            if (!categorias.Success) { return BadRequest(categorias.Message); }

            var lista = categorias.Data.Select(x => new
            {
                x.Id,
                Fecha = x.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"),
                x.Nombre,
                x.Tipo,
                x.Descripcion
            }).ToList();

            return Json(lista);

        }

        [HttpPost("categoria/crear")]
        public async Task<IActionResult> Insertar([FromBody] CategoriaDto categoria)
        {
            //TODO : Validar que no exista el nombre
            if (categoria == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            // Validaciones y lógica
            var editCategoria = new Categoria()
            {
                Fecha = categoria.Fecha,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Tipo = categoria.Tipo
            };

            var transacciones = await _categoriaService.InsertarCategoriaPersonalAsyncService(editCategoria);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

        [HttpPost("categoria/eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarDto eliminar)
        {
            if (eliminar == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }
            var transaccion = await _categoriaService.EliminarCategoriaPersonalAsyncService(eliminar.Id);
            return transaccion.Success ?
                Ok() :
                BadRequest(transaccion.Message);
        }

        [HttpPost("categoria/editar")]
        public async Task<IActionResult> Editar([FromBody] CategoriaUpdateDto categoria)
        {
            if (categoria == null)
            {
                return BadRequest("La transacción enviada es nula.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors });
            }

            // Validaciones y lógica
            var editCategoria = new Categoria()
            {
                Id = categoria.Id,
                Fecha = categoria.Fecha,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Tipo = categoria.Tipo
            };

            var transacciones = await _categoriaService.EditarCategoriaPersonalAsyncService(editCategoria);
            return transacciones.Success ?
                Ok() :
                BadRequest(transacciones.Message);
        }

    }
}
