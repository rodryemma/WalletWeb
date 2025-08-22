using Domain.Model.Entites;
using Domain.Model.Interfaces;
using Infra.DataAccess.Data;
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
    public class CuentaWalletRepository : ICuentaWalletRepository
    {
        private readonly IConnectionFactory _IConnectionFactory;
        private readonly string _connectionString;

        public CuentaWalletRepository(IConfiguration configuration, IConnectionFactory xIConnectionFactory)
        {
            _connectionString = configuration.GetConnectionString("DataBase_MySqlRasp");
            _IConnectionFactory = xIConnectionFactory;
        }

        public async Task<OperationResult<List<CuentaWallet>>> ObtenerCuentaWalletDBFullAsync()
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {

                    var sqlString = "SELECT * FROM CuentaWallet ORDER BY Nombre DESC";
                    
                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        
                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<CuentaWallet> query = new List<CuentaWallet>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new CuentaWallet()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Nombre = reader["Nombre"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    DivisaId = reader.GetInt32("DivisaId")
                                });

                            }
                            return OperationResult<List<CuentaWallet>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<CuentaWallet>>.Fail("Error al eliminar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<CuentaWallet>>.Fail("Error al eliminar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<int>> InsertarCuentaWalletAsync(CuentaWallet xCuentaWallet)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"INSERT INTO CuentaWallet (                                          
                                          Fecha,
                                          Nombre,
                                          Descripcion,
                                          DivisaId)
                                         VALUES (
                                          @Fecha,
                                          @Nombre,
                                          @Descripcion,
                                          @DivisaId)";


                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xCuentaWallet.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Nombre", xCuentaWallet.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", xCuentaWallet.Descripcion);
                        comando.Parameters.AddWithValue("@DivisaId", xCuentaWallet.DivisaId);

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

        public async Task<OperationResult<int>> EditarCuentaWalletAsync(CuentaWallet xCuentaWallet)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"UPDATE CuentaWallet 
                                 SET 
                                     Fecha = @Fecha,
                                     Nombre = @Nombre,
                                     Descripcion = @Descripcion,
                                     DivisaId = @DivisaId 
                                 WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xCuentaWallet.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Nombre", xCuentaWallet.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", xCuentaWallet.Descripcion);
                        comando.Parameters.AddWithValue("@Id", xCuentaWallet.Id);
                        comando.Parameters.AddWithValue("@DivisaId", xCuentaWallet.DivisaId);

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

        public async Task<OperationResult<int>> EliminarCuentaWalletAsync(int xId)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"DELETE FROM CuentaWallet
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
