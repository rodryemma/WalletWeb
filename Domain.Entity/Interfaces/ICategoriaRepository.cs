using Domain.Model.Entites;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<OperationResult<List<Categoria>>> ObtenerCategoriaDBFullAsync(string xTipo);
        Task<OperationResult<List<Categoria>>> ObtenerMultiplesCategoriasAsync(List<int> ids);
        Task<OperationResult<int>> InsertarCategoriaPersonalAsync(Categoria xCategoria);
        Task<OperationResult<int>> EditarCategoriaPersonalAsync(Categoria xCategoria);
        Task<OperationResult<int>> EliminarCategoriaPersonalAsync(int xId);
    }
}
