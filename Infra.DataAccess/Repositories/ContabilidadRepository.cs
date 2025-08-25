using Domain.Model.Entity;
using Domain.Model.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.Repository
{
    public class ContabilidadRepository : IContabilidadRepository
    {
        private readonly IConnectionFactory _IConnectionFactory;
        private readonly string _connectionString;

        public ContabilidadRepository(IConfiguration configuration, IConnectionFactory xIConnectionFactory)
        {
            _connectionString = configuration.GetConnectionString("DataBase_MySqlRasp");
            _IConnectionFactory = xIConnectionFactory;
        }

        public List<Contabilidad> ObtenerContabilidadDBFull(string xTipo)
        {
            using (MySqlConnection c = _IConnectionFactory.ObtenerConexionMySql(_connectionString))
            {
                try
                {
                    var sqlString = $"SELECT * FROM ContabilidadPersonal WHERE TipoMovimiento = '{xTipo.ToLower()}' ORDER BY Fecha DESC LIMIT 0,1000";

                    MySqlCommand Comando = new MySqlCommand(sqlString, c);
                    MySqlDataReader reader = Comando.ExecuteReader();
                    List<Contabilidad> query = new List<Contabilidad>();
                    while (reader.Read())
                    {
                        query.Add(new Contabilidad()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Fecha = Convert.ToDateTime(reader["Fecha"].ToString()),
                            Categoria = reader["Cateria"].ToString(),
                            Cuenta = reader["Cuenta"].ToString(),
                            CantidadDivisa = float.Parse(reader["CantidadDivisa"].ToString()),
                            Divisa = reader["Divisa"].ToString(),
                            Comentario = reader["Comentario"].ToString(),
                            TipoMovimiento = reader["TipoMovimiento"].ToString(),
                            ValorCCL = float.Parse(reader["ValorCCL"].ToString())
                        });

                    }
                    return query;
                }
                catch (MySqlException ex)
                {
                    //TODO
                    //MessageBox.Show("Se ha producido un error: " + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    //TODO
                    //MessageBox.Show("Se ha producido un error: " + ex.Message);
                    return null;
                }
                finally
                {
                    c.Close();
                }
            }

        }

        public async Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadDBFullAsync(string xTipo)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    
                    var baseSql = "SELECT * FROM ContabilidadPersonal ";
                    var whereClause = "";
                    var parametros = new List<MySqlParameter>();

                    if (!string.Equals(xTipo, "total", StringComparison.OrdinalIgnoreCase))
                    {
                        whereClause = " WHERE TipoMovimiento = @Tipo ";
                        parametros.Add(new MySqlParameter("@Tipo", xTipo.ToLower()));
                    }

                    var sqlString = $"{baseSql}{whereClause} ORDER BY Fecha DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        if (parametros.Any())
                            Comando.Parameters.AddRange(parametros.ToArray());

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Contabilidad> query = new List<Contabilidad>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Contabilidad()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Categoria = reader["Cateria"].ToString(),
                                    Cuenta = reader["Cuenta"].ToString(),
                                    CantidadDivisa = reader.GetFloat("CantidadDivisa"),
                                    Divisa = reader["Divisa"].ToString(),
                                    Comentario = reader["Comentario"].ToString(),
                                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                                    ValorCCL = reader.GetFloat("ValorCCL"),
                                    DivisaId = reader.GetInt32("DivisaId"),
                                    CuentaWalletId = reader.GetInt32("CuentaWalletId")
                                });

                            }
                            return OperationResult<List<Contabilidad>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al eliminar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al eliminar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadDBFullAsync(string xTipo, DateTime xFechaDesde)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    var baseSql = "SELECT * FROM ContabilidadPersonal WHERE  Fecha >=@Fecha ";
                    var whereClause = "";
                    var parametros = new List<MySqlParameter>();
                    parametros.Add(new MySqlParameter("@Fecha", xFechaDesde.ToString("yyyy-MM-dd HH:mm:ss")));
                    if (!string.Equals(xTipo, "total", StringComparison.OrdinalIgnoreCase))
                    {
                        whereClause = " AND TipoMovimiento = @Tipo";
                        parametros.Add(new MySqlParameter("@Tipo", xTipo.ToLower()));
                    }

                    var sqlString = $"{baseSql}{whereClause} ORDER BY Fecha DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        if (parametros.Any())
                            Comando.Parameters.AddRange(parametros.ToArray());

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Contabilidad> query = new List<Contabilidad>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Contabilidad()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Categoria = reader["Cateria"].ToString(),
                                    Cuenta = reader["Cuenta"].ToString(),
                                    CantidadDivisa = reader.GetFloat("CantidadDivisa"),
                                    Divisa = reader["Divisa"].ToString(),
                                    Comentario = reader["Comentario"].ToString(),
                                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                                    ValorCCL = reader.GetFloat("ValorCCL"),
                                    DivisaId = reader.GetInt32("DivisaId"),
                                    CuentaWalletId = reader.GetInt32("CuentaWalletId")
                                });

                            }
                            return OperationResult<List<Contabilidad>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al eliminar: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al eliminar: " + ex.Message);
                }
            }

        }

        public async Task<OperationResult<List<Contabilidad>>> ObtenerContabilidadJoinDBFullAsync(string xTipo, DateTime xFechaDesde)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    var baseSql = "SELECT CP.Id, CP.Fecha, CP.CantidadDivisa, CP.Comentario, CP.TipoMovimiento, CP.ValorCCL, CP.DivisaId, CP.CuentaWalletId, CP.CategoriaId, " +
                        "C.Nombre AS Categoria, " +
                        "D.Nombre AS Divisa, " +
                        "CW.Nombre AS Cuenta " +
                        "FROM ContabilidadPersonal AS CP " +
                        "INNER JOIN Categoria AS C ON (C.Id = CP.CategoriaId)  " +
                        "INNER JOIN Divisa AS D ON (D.Id = CP.DivisaId) " +
                        "INNER JOIN CuentaWallet AS CW ON (CW.Id = CP.CuentaWalletId) " +
                        "WHERE  CP.Fecha >= @Fecha ";
                    var whereClause = "";
                    var parametros = new List<MySqlParameter>();
                    parametros.Add(new MySqlParameter("@Fecha", xFechaDesde.ToString("yyyy-MM-dd HH:mm:ss")));
                    if (!string.Equals(xTipo, "total", StringComparison.OrdinalIgnoreCase))
                    {
                        whereClause = " AND TipoMovimiento = @Tipo";
                        parametros.Add(new MySqlParameter("@Tipo", xTipo.ToLower()));
                    }

                    var sqlString = $"{baseSql}{whereClause} ORDER BY Fecha DESC";

                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        if (parametros.Any())
                            Comando.Parameters.AddRange(parametros.ToArray());

                        using (MySqlDataReader reader = await Comando.ExecuteReaderAsync())
                        {
                            List<Contabilidad> query = new List<Contabilidad>();
                            while (await reader.ReadAsync())
                            {
                                query.Add(new Contabilidad()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Categoria = reader["Categoria"].ToString(),
                                    Cuenta = reader["Cuenta"].ToString(),
                                    CantidadDivisa = reader.GetFloat("CantidadDivisa"),
                                    Divisa = reader["Divisa"].ToString(),
                                    Comentario = reader["Comentario"].ToString(),
                                    TipoMovimiento = reader["TipoMovimiento"].ToString(),
                                    ValorCCL = reader.GetFloat("ValorCCL"),
                                    CategoriaId = reader.GetInt32("CategoriaId"),
                                    DivisaId = reader.GetInt32("DivisaId"),
                                    CuentaWalletId = reader.GetInt32("CuentaWalletId")
                                });

                            }
                            return OperationResult<List<Contabilidad>>.Ok(query);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al obtener consulta: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return OperationResult<List<Contabilidad>>.Fail("Error al obtener consulta: " + ex.Message);
                }
            }

        }

        public DataTable ObtenerContabilidadDBFull()
        {
            using (MySqlConnection c = _IConnectionFactory.ObtenerConexionMySql(_connectionString))
            {
                try
                {
                    var sqlString = $"SELECT * FROM ContabilidadPersonal ORDER BY Fecha DESC";
                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter(Comando);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch (MySqlException ex)
                {
                    //TODO
                    //MessageBox.Show("Se ha producido un error: " + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    //TODO
                    //MessageBox.Show("Se ha producido un error: " + ex.Message);
                    return null;
                }
                finally
                {
                    c.Close();
                }
            }
        }

        public async Task<int> InsertarContabilidadPersonalAsync(Contabilidad xContabilidad, string xValorCCL)
        {
            int respt = 0;

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    var sqlString = $"INSERT INTO ContabilidadPersonal (Fecha,Cateria,Cuenta,CantidadDivisa,Divisa,Comentario,TipoMovimiento,ValorCCL) " +
                        $"VALUES ( '{xContabilidad.Fecha.ToString("yyyy-MM-dd HH:mm:ss")}','{xContabilidad.Categoria}','{xContabilidad.Cuenta}'," +
                        $"'{xContabilidad.CantidadDivisa.ToString().Replace(",", ".")}','{xContabilidad.Divisa}'," +
                        $"'{xContabilidad.Comentario}','{xContabilidad.TipoMovimiento}'," +
                        $"'{xValorCCL.ToString().Replace(",", ".")}')";



                    using (MySqlCommand Comando = new MySqlCommand(sqlString, c))
                    {
                        await Comando.ExecuteNonQueryAsync();
                    }
                    respt = 1;

                }
                catch (MySqlException ex)
                {
                    //TODO
                    //MessageBox.Show("Error al InsertarContabilidadPersonal: " + ex.Message);
                    respt = 0;
                }
                finally
                {
                    c.Close();
                }
            }
            return respt;
        }

        public async Task<OperationResult<int>> EditarContabilidadPersonalAsync(Contabilidad xContabilidad)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"UPDATE ContabilidadPersonal 
                                 SET 
                                     Fecha = @Fecha,
                                     Cateria = @Categoria,
                                     Cuenta = @Cuenta,
                                     CantidadDivisa = @CantidadDivisa,
                                     Divisa = @Divisa,
                                     Comentario = @Comentario,
                                     TipoMovimiento = @TipoMovimiento,
                                     ValorCCL = @ValorCCL,
                                     DivisaId = @DivisaId,
                                     CuentaWalletId = @CuentaWalletId
                                 WHERE Id = @Id";

                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xContabilidad.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Categoria", xContabilidad.Categoria);
                        comando.Parameters.AddWithValue("@Cuenta", xContabilidad.Cuenta);
                        comando.Parameters.AddWithValue("@CantidadDivisa", xContabilidad.CantidadDivisa);
                        comando.Parameters.AddWithValue("@Divisa", xContabilidad.Divisa);
                        comando.Parameters.AddWithValue("@Comentario", xContabilidad.Comentario);
                        comando.Parameters.AddWithValue("@TipoMovimiento", xContabilidad.TipoMovimiento);
                        comando.Parameters.AddWithValue("@ValorCCL", xContabilidad.ValorCCL);
                        comando.Parameters.AddWithValue("@Id", xContabilidad.Id);
                        comando.Parameters.AddWithValue("@DivisaId", xContabilidad.DivisaId);
                        comando.Parameters.AddWithValue("@CuentaWalletId", xContabilidad.CuentaWalletId);

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

        public async Task<OperationResult<int>> InsertarContabilidadPersonalAsync(Contabilidad xContabilidad)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"INSERT INTO ContabilidadPersonal (                                          
                                          Fecha,
                                          Cateria,
                                          Cuenta,
                                          CantidadDivisa,
                                          Divisa,
                                          Comentario,
                                          TipoMovimiento,
                                          ValorCCL,
                                          DivisaId,
                                          CuentaWalletId)
                                         VALUES (
                                          @Fecha,
                                          @Categoria,
                                          @Cuenta,
                                          @CantidadDivisa,
                                          @Divisa,
                                          @Comentario,
                                          @TipoMovimiento,
                                          @ValorCCL,
                                          @DivisaId,
                                          @CuentaWalletId)";


                    using (MySqlCommand comando = new MySqlCommand(sqlString, c))
                    {
                        comando.Parameters.AddWithValue("@Fecha", xContabilidad.Fecha.ToString("yyyy-MM-dd HH:mm:ss"));
                        comando.Parameters.AddWithValue("@Categoria", xContabilidad.Categoria);
                        comando.Parameters.AddWithValue("@Cuenta", xContabilidad.Cuenta);
                        comando.Parameters.AddWithValue("@CantidadDivisa", xContabilidad.CantidadDivisa);
                        comando.Parameters.AddWithValue("@Divisa", xContabilidad.Divisa);
                        comando.Parameters.AddWithValue("@Comentario", xContabilidad.Comentario);
                        comando.Parameters.AddWithValue("@TipoMovimiento", xContabilidad.TipoMovimiento);
                        comando.Parameters.AddWithValue("@ValorCCL", xContabilidad.ValorCCL);
                        comando.Parameters.AddWithValue("@DivisaId", xContabilidad.DivisaId);
                        comando.Parameters.AddWithValue("@CuentaWalletId", xContabilidad.CuentaWalletId);

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

        public async Task<OperationResult<int>> EliminarContabilidadPersonalAsync(int xId)
        {

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync(_connectionString))
            {
                try
                {
                    string sqlString = @"DELETE FROM ContabilidadPersonal
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
