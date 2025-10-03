using Domain.Model.Entites;
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
    public class DivisaRepository : IDivisaRepository
    {
        private readonly IConnectionFactory _IConnectionFactory;
        private readonly string _connectionString;

        public DivisaRepository(IConnectionFactory iConnectionFactory, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DataBase_MySqlRasp");
            _IConnectionFactory = iConnectionFactory;
        }

        public async Task<OperationResult<List<Divisa>>> ObtenerDivisaDBFullAsync()
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {

                    var sqlString = "SELECT * FROM Divisa ORDER BY Nombre DESC";
                    
                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Divisa> query = new List<Divisa>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Divisa()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Divisa>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Divisa>>> ObtenerMultiplesDivisasAsync(List<int> ids)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                if (ids == null || !ids.Any())
                {
                    return OperationResult<List<Divisa>>.Ok(new List<Divisa>());
                }

                try
                {

                    var parametros = string.Join(",", ids.Select((id, index) => $"@id{index}"));
                    var sqlString = $"SELECT * FROM Divisa WHERE Id IN ({parametros}) ORDER BY Nombre DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            Comando.Parameters.AddWithValue($"@id{i}", ids[i]);
                        }

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Divisa> query = new List<Divisa>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Divisa()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Divisa>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Divisa>>> ObtenerMultiplesDivisasAsync(List<string> nombres)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                if (nombres == null || !nombres.Any())
                {
                    return OperationResult<List<Divisa>>.Ok(new List<Divisa>());
                }

                try
                {

                    var parametros = string.Join(",", nombres.Select((id, index) => $"@id{index}"));
                    var sqlString = $"SELECT * FROM Divisa WHERE Nombre IN ({parametros}) ORDER BY Nombre DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        for (int i = 0; i < nombres.Count; i++)
                        {
                            Comando.Parameters.AddWithValue($"@id{i}", nombres[i]);
                        }

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Divisa> query = new List<Divisa>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Divisa()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });

                            }
                            return OperationResult<List<Divisa>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Divisa>>.Fail("Error al buscar: " + ex.Message);
                }
            }

        }
        public async Task<OperationResult<int>> InsertarDivisaAsync(Divisa xDivisa)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"INSERT INTO Divisa (        
                                          Nombre,
                                          Descripcion)
                                         VALUES (
                                          @Nombre,
                                          @Descripcion)";


                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Nombre", xDivisa.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", xDivisa.Descripcion);

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

        public async Task<OperationResult<int>> EditarDivisaAsync(Divisa xDivisa)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"UPDATE Divisa 
                                 SET 
                                     Nombre = @Nombre,
                                     Descripcion = @Descripcion
                                 WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Nombre", xDivisa.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", xDivisa.Descripcion);
                        comando.Parameters.AddWithValue("@Id", xDivisa.Id);

                        var filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Editado correctamente");
                        else
                            return OperationResult<int>.Fail("No se editó el registro");
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al editar: " + ex.Message);
                }
            }
        }

        public async Task<OperationResult<int>> EliminarDivisaAsync(int xId)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"DELETE FROM Divisa
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
