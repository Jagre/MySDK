using Dapper.Contrib.Extensions;
using MySDK.Configuration;
using MySDK.Dapper.Extensions;
using MySDK.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;


namespace MySDK.Dapper
{
    public static class DapperBase
    {
        /// <summary>
        /// constructor
        /// </summary>
        static DapperBase()
        {
            SqlMapperExtensions.TableNameMapper = (type) => type.Name;
        }

        /// <summary>
        /// paging sql script template, support mssql, mysql(>=8.0, windows func),
        /// {0}: complex sql query,
        /// {1}: order by fields' name
        /// {2}, {3}: the boundary value according to count by pageIndex & pageSize
        /// </summary>
        public const string PAGING_SQL_SCRIPT_TEMPLATE = @" 
            WITH 
                _data AS (
                    {0}
                ),
                _count AS (SELECT COUNT(0) AS TotalCount FROM _data)

            SELECT  *   
            FROM    (SELECT *, ROW_NUMBER() OVER (ORDER BY {1}) AS Row_No FROM _data  CROSS JOIN _count) x
            WHERE   Row_No >= {2} AND Row_No <= {3}
            ORDER BY {1}";

        /// <summary>
        /// get the instance of IDbConnection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">connection name in configuration file</param>
        /// <returns></returns>
        public static IDbConnection GetConnection<T>(string connectionName) where T : IDbConnection
        {
            return MyServiceProvider.Configuration.GetConnectionConfig(connectionName).GetDbConnection<T>();
        }

        public static IDbConnection GetSqlConnection(string connectionName)
        {
            return GetConnection<SqlConnection>(connectionName);
        }

        public static IDbConnection GetMySqlConnection(string connectionName)
        {
            return GetConnection<MySqlConnection>(connectionName);
        }

        
    }
}
