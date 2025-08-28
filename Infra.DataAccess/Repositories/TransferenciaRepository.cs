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
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly IConnectionFactory _IConnectionFactory;
        private readonly string _connectionString;

        public TransferenciaRepository(IConnectionFactory iConnectionFactory, IConfiguration configuration)
        {
            _IConnectionFactory = iConnectionFactory;
            _connectionString = configuration.GetConnectionString("DataBase_MySqlRasp");
        }

        public async Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaDBFullAsync()
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {

                    var sqlString = "SELECT * FROM Transferencia ORDER BY Fecha DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Transferencia> query = new List<Transferencia>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Transferencia()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    CuentaEnviaId = reader.GetInt32("CuentaEnviaId"),
                                    CuentaRecibeId = reader.GetInt32("CuentaRecibeId"),
                                    Monto = reader.GetDecimal("Monto"),
                                    Comentario = reader["Comentario"].ToString()
                                });

                            }
                            return OperationResult<List<Transferencia>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Transferencia>>.Fail("Error al consultar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Transferencia>>.Fail("Error al consultar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Transferencia>>> ObtenerTransferenciaJoinDBFullAsync(DateTime xFechaDesde)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    var baseSql = "SELECT T.Id, T.CuentaEnviaId, CWE.Nombre AS CuentaEnvia, T.CuentaRecibeId, CWB.Nombre AS CuentaRecibe , T.Monto, T.Fecha, T.Comentario " +
                        "FROM Transferencia AS T " +
                        "INNER JOIN CuentaWallet AS CWE ON (CWE.Id = T.CuentaEnviaId) " +
                        "INNER JOIN CuentaWallet AS CWB ON (CWB.Id = T.CuentaRecibeId) " +
                        "WHERE  T.Fecha >= @Fecha ";
                    var whereClause = "";
                    var parametros = new List<MySqlParameter>();
                    parametros.Add(new MySqlParameter("@Fecha", xFechaDesde.ToString("yyyy-MM-dd HH:mm:ss")));
                    
                    var sqlString = $"{baseSql}{whereClause} ORDER BY Fecha DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        if (parametros.Any())
                            Comando.Parameters.AddRange(parametros.ToArray());

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Transferencia> query = new List<Transferencia>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Transferencia()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    CuentaEnvia = reader["CuentaEnvia"].ToString(),
                                    CuentaEnviaId = reader.GetInt32("CuentaEnviaId"),
                                    CuentaRecibe = reader["CuentaRecibe"].ToString(),
                                    CuentaRecibeId = reader.GetInt32("CuentaRecibeId"),
                                    Comentario = reader["Comentario"].ToString(),
                                    Monto = reader.GetDecimal("Monto")
                                });

                            }
                            return OperationResult<List<Transferencia>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Transferencia>>.Fail("Error al obtener consulta: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Transferencia>>.Fail("Error al obtener consulta: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<int>> InsertarTransferenciaAsync(Transferencia xTransferencia)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"INSERT INTO Transferencia (                                          
                                          Fecha,
                                          CuentaEnviaId,
                                          CuentaRecibeId,
                                          Monto,
                                          Comentario)
                                         VALUES (
                                          @Fecha,
                                          @CuentaEnviaId,
                                          @CuentaRecibeId,
                                          @Monto,
                                          @Comentario)";


                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xTransferencia.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@CuentaEnviaId", xTransferencia.CuentaEnviaId);
                        comando.Parameters.AddWithValue("@CuentaRecibeId", xTransferencia.CuentaRecibeId);
                        comando.Parameters.AddWithValue("@Monto", xTransferencia.Monto);
                        comando.Parameters.AddWithValue("@Comentario", xTransferencia.Comentario);

                        var filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Insertado correctamente");
                        else
                            return OperationResult<int>.Fail("No se insertó el registro");
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al eliminar: " + ex.Message);
                }
            }
        }

        public async Task<OperationResult<int>> EditarTransferenciaAsync(Transferencia xTransferencia)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"UPDATE Transferencia 
                                 SET 
                                     Fecha = @Fecha,
                                     CuentaEnviaId = @CuentaEnviaId,
                                     CuentaRecibeId = @CuentaRecibeId,
                                     Monto = @Monto,
                                     Comentario = @Comentario
                                 WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xTransferencia.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@CuentaEnviaId", xTransferencia.CuentaEnviaId);
                        comando.Parameters.AddWithValue("@CuentaRecibeId", xTransferencia.CuentaRecibeId);
                        comando.Parameters.AddWithValue("@Monto", xTransferencia.Monto);
                        comando.Parameters.AddWithValue("@Comentario", xTransferencia.Comentario);
                        comando.Parameters.AddWithValue("@Id", xTransferencia.Id);

                        var filasRta = await comando.ExecuteNonQueryAsync();

                        if (filasRta > 0)
                            return OperationResult<int>.Ok(filasRta, "Editado correctamente");
                        else
                            return OperationResult<int>.Fail("No se editó el registro");
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<int>.Fail("Error al eliminar: " + ex.Message);
                }
            }
        }

        public async Task<OperationResult<int>> EliminarTransferenciaAsync(int xId)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"DELETE FROM Transferencia
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
