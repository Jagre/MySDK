using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace MySDK.Dapper.Sql
{
    public class DapperContext<T> where T : IDbConnection
    {
        public IDbConnection GetDbConnection(IConfigurationRoot config, string connectionName)
        {
            return GetDbConnection(config, connectionName);
        }

        public IDbConnection GetDbConnection(string connectionString)
        {
            return connectionString.GetDbConnection<T>();
        }
    }

    public static class DbConfigurationExtension
    {
        public static IDbConnection GetDbConnection<T>(IConfigurationRoot config, string connectinName) where T : IDbConnection
        {
            return config.GetConnectionString(connectinName).GetDbConnection<T>();
        }

        public static IDbConnection GetDbConnection<T>(this string connectionString) where T : IDbConnection
        {
            if (typeof(T).Equals(typeof(SqlConnection)))
            {
                return new SqlConnection(connectionString);
            }
            else if (typeof(T).Equals(typeof(MySqlConnection)))
            {
                return new MySqlConnection(connectionString);
            }
            return default(T);
        }

        public static T ReadUncommitted<T>(this T connection) where T : IDbConnection
        {
            connection.Execute("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            return connection;
        }
    }
}
