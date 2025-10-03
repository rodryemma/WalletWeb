using Application.Interfaces;
using Domain.Model.Entites;
using Domain.Model.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        ICategoriaRepository _CategoriaRepository;
        public CategoriaService(ICategoriaRepository xCategoriaRepository)
        {
            _CategoriaRepository = xCategoriaRepository;
        }

        public Task<OperationResult<List<Categoria>>> ObtenerCategoriaDBFullAsyncService(string xTipo)
        {
            return _CategoriaRepository.ObtenerCategoriaDBFullAsync(xTipo);
        }

        public Task<OperationResult<List<Categoria>>> ObtenerMultiplesCategoriasAsyncService(List<int> xIds)
        {
            return _CategoriaRepository.ObtenerMultiplesCategoriasAsync(xIds);
        }

        public Task<OperationResult<List<Categoria>>> ObtenerMultiplesCategoriasAsyncService(List<string> xNombres, string xTipo)
        {
            return _CategoriaRepository.ObtenerMultiplesCategoriasAsync(xNombres, xTipo);
        }

        public Task<OperationResult<int>> EditarCategoriaPersonalAsyncService(Categoria xCategoria)
        {
            return _CategoriaRepository.EditarCategoriaPersonalAsync(xCategoria);
        }

        public Task<OperationResult<int>> EliminarCategoriaPersonalAsyncService(int xId)
        {
            return _CategoriaRepository.EliminarCategoriaPersonalAsync(xId);
        }

        public Task<OperationResult<int>> InsertarCategoriaPersonalAsyncService(Categoria xCategoria)
        {
            return _CategoriaRepository.InsertarCategoriaPersonalAsync(xCategoria);
        }
        
    }
}
