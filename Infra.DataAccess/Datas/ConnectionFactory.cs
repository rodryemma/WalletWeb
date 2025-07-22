using Microsoft.Extensions.Configuration;
using Domain.Model;
using Domain.Model.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;


namespace Infra.DataAccess.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        //static readonly string _DBMySql = "DataBase_MySqlRasp";
        //static readonly string _DBSqlServer = "DataBase_MySqlRasp";



        #region Mysql
        public MySqlConnection ObtenerConexionMySql(string xCadena)
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(ObtenerCadena(xCadena));
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                var sDataError = ObtenerCadena(xCadena).Split(';');
                //TODO
                //MessageBox.Show("No se conecto con la base de datos: " + sDataError[0]);
                //MessageBox.Show("Error: " + ex.Message);
                return null;
            }

        }

        public async Task<MySqlConnection> ObtenerConexionMySqlAsync(string xCadena)
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(ObtenerCadena(xCadena));
                await conexion.OpenAsync();
                return conexion;
            }
            catch (Exception ex)
            {
                //TODO
                //MessageBox.Show(ObtenerCadena(_DBMySql));
                //MessageBox.Show("No se conecto con la base de datos: " + ex.ToString());
                return null;
            }

        }

        #endregion

        #region SqlServer

        //public SqlConnection ObtenerConexionSqlServer()
        //{
        //    try
        //    {
        //        SqlConnection conexion = new SqlConnection(ObtenerCadena(_DBSqlServer));
        //        conexion.Open();
        //        return conexion;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ObtenerCadena(_DBSqlServer));
        //        MessageBox.Show("No se conecto con la base de datos: " + ex.ToString());
        //        return null;
        //    }

        //}

        //public async Task<SqlConnection> ObtenerConexionSqlServerAsync()
        //{
        //    try
        //    {
        //        SqlConnection conexion = new SqlConnection(ObtenerCadena(_DBSqlServer));
        //        await conexion.OpenAsync();
        //        return conexion;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ObtenerCadena(_DBSqlServer));
        //        MessageBox.Show("No se conecto con la base de datos: " + ex.ToString());
        //        return null;
        //    }

        //}

        #endregion

        public static string ObtenerCadena(string xCadena)
        {

            String urlconectionString = xCadena;

            String server = "";
            String user = "";
            String Password = "";
            String db = "";

            //Se desarma String de conexion en App.config
            string[] arrElementos = urlconectionString.Split(';');
            foreach (var s in arrElementos)
            {
                string[] arrValue = s.Split('=');
                switch (arrValue[0].ToUpper())
                {
                    case "DATA SOURCE":
                        server = arrValue[1];
                        break;
                    case "USER ID":
                        user = arrValue[1];
                        break;
                    case "PASSWORD":
                        Password = arrValue[1];
                        break;
                    case "INITIAL CATALOG":
                        db = arrValue[1];
                        break;
                }
            }


            //Se crea cadena de string para la conexion
            string connString = @"Data Source=" + server + ";Initial Catalog="
                        + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + Password;

            return connString;
        }
    }
}
