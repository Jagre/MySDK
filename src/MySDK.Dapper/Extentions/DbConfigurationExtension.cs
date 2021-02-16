using Dapper;
using Microsoft.Extensions.Configuration;
using MySDK.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace MySDK.Dapper.Extentions
{
    public static class DbConfigurationExtension
    {
        public static IDbConnection GetDbConnection<T>(IConfiguration config, string connectinName) where T : IDbConnection
        {
            return config.GetConnectionConfig(connectinName).GetDbConnection<T>();
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
