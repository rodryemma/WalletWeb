using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<OperationResult<List<Categoria>>> ObtenerCategoriaDBFullAsyncService(string xTipo);
        Task<OperationResult<int>> InsertarCategoriaPersonalAsyncService(Categoria xCategoria);
        Task<OperationResult<int>> EditarCategoriaPersonalAsyncService(Categoria xCategoria);
        Task<OperationResult<int>> EliminarCategoriaPersonalAsyncService(int xId);
    }
}
