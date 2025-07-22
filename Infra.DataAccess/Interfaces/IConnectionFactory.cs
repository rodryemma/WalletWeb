using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess.Interface
{
    public interface IConnectionFactory
    {
        MySqlConnection ObtenerConexionMySql();

        Task<MySqlConnection> ObtenerConexionMySqlAsync();
    }
}
