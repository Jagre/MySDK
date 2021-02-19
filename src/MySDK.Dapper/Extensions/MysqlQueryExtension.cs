using Dapper;
using MySDK.Basic.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySDK.Dapper.Extensions
{
    public static class MysqlQueryExtension
    {
        public static async Task<T> GetAsync<T>(this string sql, string connectionName, object param = null, IDbTransaction tran = null)
        {
            using (var conn = DapperBase.GetMySqlConnection(connectionName))
            {
                var result = await conn.ReadUncommitted().QueryAsync<T>(sql, param, tran);
                return result.FirstOrDefault();
            }
        }

        public static async Task<bool> ExecuteAsync(this string sql, string connectionName, object param =null, IDbTransaction tran = null)
        {
            using (var conn = DapperBase.GetMySqlConnection(connectionName))
            {
                var effectCount = await conn.ExecuteAsync(sql, param, tran);
                return effectCount > 0;
            }
        }

        public static async Task<PagingResult<T>> PagingAsync<T>(this string sql, string connectionName, string orderByFields, int pageIndex = 1, int pageSize = 15, object param = null)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 15;

            try
            {
                PagingResult<T> result = new PagingResult<T>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var pagingSql = string.Format(DapperBase.PAGING_SQL_SCRIPT_TEMPLATE, sql, orderByFields, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                using (var conn = DapperBase.GetMySqlConnection(connectionName))
                {
                    result.Items = (await conn.ReadUncommitted().QueryAsync<T, long, T>(pagingSql,
                        (a, b) => { result.TotalCount = b; return a; },
                        param,
                        splitOn: "TotalCount")).AsList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }
    }
}
