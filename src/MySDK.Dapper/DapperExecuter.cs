using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using MySDK.Dapper.Sql;
using MySDK.DependencyInjection;
using System.Data;

namespace MySDK.Dapper
{
    public static class DapperExecuter
    {
        /// <summary>
        /// constructor
        /// </summary>
        static DapperExecuter()
        {
            SqlMapperExtensions.TableNameMapper = (type) => type.Name;
        }

        public const string MSSQL_PAGING_SCRIPT_TEMPLATE = @" 
            WITH 
                _data AS ({0}),
                _count AS (SELECT COUNT(0) AS OverallCount FROM _data)

            SELECT  *   
            FROM    (SELECT *, ROW_NUMBER() OVER (ORDER BY {1}) AS row_num FROM _data  CROSS APPLY _count) x
            WHERE   row_num BETWEEN {2} AND {3}
            ORDER BY {1}";

        public const string MYSQL_PAGING_SCRIPT_TEMPLATE = @"
            
        ";

        /// <summary>
        /// get the instance of IDbConnection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">connection name in configuration file</param>
        /// <returns></returns>
        public static IDbConnection GetConnection<T>(string connectionName) where T : IDbConnection
        {
            return MyServiceProvider.Configuration.GetConnectionString(connectionName).GetDbConnection<T>();
        }

    }
}
