using Domain.Model.Entites;
using Domain.Model.Entity;
using Domain.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IConnectionFactory _IConnectionFactory;
        private readonly string _connectionString;

        public CategoriaRepository(IConfiguration configuration, IConnectionFactory xIConnectionFactory)
        {
            _connectionString = configuration.GetConnectionString("DataBase_MySqlRasp");
            _IConnectionFactory = xIConnectionFactory;
        }

        public async Task<OperationResult<List<Categoria>>> ObtenerCategoriaDBFullAsync(string xTipo)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {

                    var baseSql = "SELECT * FROM Categoria ";
                    var whereClause = "";
                    var parametros = new List<MySqlParameter>();

                    if (!string.Equals(xTipo, "total", StringComparison.OrdinalIgnoreCase))
                    {
                        whereClause = " WHERE Tipo = @Tipo ";
                        parametros.Add(new MySqlParameter("@Tipo", xTipo.ToLower()));
                    }

                    var sqlString = $"{baseSql}{whereClause} ORDER BY Nombre DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        if (parametros.Any())
                            Comando.Parameters.AddRange(parametros.ToArray());

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Categoria> query = new List<Categoria>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Categoria()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Tipo = reader["Tipo"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Categoria>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al consultar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al consultar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Categoria>>> ObtenerMultiplesCategoriasAsync(List<int> ids)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                if (ids == null || !ids.Any())
                {
                    return OperationResult<List<Categoria>>.Ok(new List<Categoria>());
                }

                try
                {

                    var parametros = string.Join(",", ids.Select((id, index) => $"@id{index}"));
                    var sqlString = $"SELECT * FROM Categoria WHERE Id IN ({parametros}) ORDER BY Nombre DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            Comando.Parameters.AddWithValue($"@id{i}", ids[i]);
                        }

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Categoria> query = new List<Categoria>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Categoria()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Tipo = reader["Tipo"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Categoria>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al buscar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al buscar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Categoria>>> ObtenerMultiplesCategoriasAsync(List<string> nombres, string tipo)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                if (nombres == null || !nombres.Any())
                {
                    return OperationResult<List<Categoria>>.Ok(new List<Categoria>());
                }

                try
                {

                    var parametros = string.Join(",", nombres.Select((id, index) => $"@id{index}"));
                    var sqlString = $"SELECT * FROM Categoria WHERE Nombre IN ({parametros}) AND Tipo = '{tipo}' ORDER BY Nombre DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        for (int i = 0; i < nombres.Count; i++)
                        {
                            Comando.Parameters.AddWithValue($"@id{i}", nombres[i]);
                        }

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Categoria> query = new List<Categoria>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Categoria()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Tipo = reader["Tipo"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Categoria>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al buscar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Categoria>>.Fail("Error al buscar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<int>> InsertarCategoriaPersonalAsync(Categoria xCategoria)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"INSERT INTO Categoria (                                          
                                          Fecha,
                                          Nombre,
                                          Tipo,
                                          Descripcion)
                                         VALUES (
                                          @Fecha,
                                          @Nombre,
                                          @Tipo,
                                          @Descripcion)";


                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xCategoria.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Nombre", xCategoria.Nombre);
                        comando.Parameters.AddWithValue("@Tipo", xCategoria.Tipo);
                        comando.Parameters.AddWithValue("@Descripcion", xCategoria.Descripcion);

                        var filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Insertado correctamente");
                        else
                            return OperationResult<int>.Fail("No se insertó el registro");
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al insertar: " + ex.Message);
                }
            }
        }

        public async Task<OperationResult<int>> EditarCategoriaPersonalAsync(Categoria xCategoria)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"UPDATE Categoria 
                                 SET 
                                     Fecha = @Fecha,
                                     Nombre = @Nombre,
                                     Tipo = @Tipo,
                                     Descripcion = @Descripcion
                                 WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xCategoria.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Nombre", xCategoria.Nombre);
                        comando.Parameters.AddWithValue("@Tipo", xCategoria.Tipo);
                        comando.Parameters.AddWithValue("@Descripcion", xCategoria.Descripcion);
                        comando.Parameters.AddWithValue("@Id", xCategoria.Id);

                        var filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Editado correctamente");
                        else
                            return OperationResult<int>.Fail("No se editó el registro");
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al actualizar: " + ex.Message);
                }
            }
        }

        public async Task<OperationResult<int>> EliminarCategoriaPersonalAsync(int xId)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"DELETE FROM Categoria
                                          WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Id", xId);

                        int filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Eliminado correctamente");
                        else
                            return OperationResult<int>.Fail("No se encontró el registro para eliminar");

                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al eliminar: " + ex.Message);
                }
            }

        }

    }
}
