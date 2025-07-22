using Domain.Model.Entity;
using Infra.DataAccess.Interface;
using MySqlConnector;
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

        public ContabilidadRepository(IConnectionFactory xIConnectionFactory)
        {
            _IConnectionFactory = xIConnectionFactory;
        }

        public List<Contabilidad> ObtenerContabilidadDBFull(string xTipo)
        {
            using (MySqlConnection c = _IConnectionFactory.ObtenerConexionMySql())
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

        public async Task<List<Contabilidad>> ObtenerContabilidadDBFullAsync(string xTipo)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync())
            {
                try
                {
                    var sqlString = $"SELECT * FROM ContabilidadPersonal WHERE TipoMovimiento = '{xTipo.ToLower()}' ORDER BY Fecha DESC";

                    MySqlCommand Comando = new MySqlCommand(sqlString, c);
                    MySqlDataReader reader = await Comando.ExecuteReaderAsync();
                    List<Contabilidad> query = new List<Contabilidad>();
                    while (await reader.ReadAsync())
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
        public async Task<List<Contabilidad>> ObtenerContabilidadDBFullAsync(string xTipo, DateTime xFechaDesde)
        {
            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync())
            {
                try
                {
                    var sqlString = $"SELECT * FROM ContabilidadPersonal " +
                        $"WHERE TipoMovimiento = '{xTipo.ToLower()}' " +
                        $"AND Fecha >= '{xFechaDesde.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                        $"ORDER BY Fecha DESC";

                    MySqlCommand Comando = new MySqlCommand(sqlString, c);
                    MySqlDataReader reader = await Comando.ExecuteReaderAsync();
                    List<Contabilidad> query = new List<Contabilidad>();
                    while (await reader.ReadAsync())
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

        public DataTable ObtenerContabilidadDBFull()
        {
            using (MySqlConnection c = _IConnectionFactory.ObtenerConexionMySql())
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

            using (MySqlConnection c = await _IConnectionFactory.ObtenerConexionMySqlAsync())
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
    }
}
