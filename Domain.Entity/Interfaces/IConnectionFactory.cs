using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Interfaces
{
    public interface IConnectionFactory
    {
        MySqlConnection ObtenerConexionMySql(string xCadena);

        Task<MySqlConnection> ObtenerConexionMySqlAsync(string xCadena);
    }
}
