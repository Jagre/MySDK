using Dapper;
using Dapper.Contrib.Extensions;
using MySDK.Basic.Models;
using MySDK.Dapper.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySDK.Dapper
{
    /// <summary>
    /// mysql repository
    /// </summary>
    /// <typeparam name="TTable"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MySqlDapperRepository<TTable, TKey> : DapperRepositoryBase<MySqlConnection>, IDapperRepository<TTable, TKey>, IDisposable
        where TTable : class
        where TKey : struct
    {
        public MySqlDapperRepository(string connectionName)
            : base(connectionName)
        {
        }

        public async Task<bool> DeleteAsync(TTable entity, IDbTransaction tran = null)
        {
            return await Connection.DeleteAsync(entity, tran);
        }

        public async Task<bool> DeleteAsync(List<TTable> entities, IDbTransaction tran = null)
        {
            return await Connection.DeleteAsync(entities, tran);
        }

        public async Task<bool> DeleteAsync(string whereAfterQueryString, object param = null, IDbTransaction tran = null)
        {
            return await Connection.ExecuteAsync($"DELETE FROM {typeof(TTable).Name} {FullWhereQueryCondition(whereAfterQueryString)}", param, tran) > 0;
        }

        public async Task<TTable> GetAsync(TKey id)
        {
            return await Connection.ReadUncommitted().GetAsync<TTable>(id);
        }

        public async Task<List<TTable>> GetAsync(List<TKey> ids)
        {
            if (ids == null || !ids.Any())
                return new List<TTable>();

            var type = typeof(TTable);
            var primaryKey = type.GetPrimaryKeyName();

            List<TTable> result = new List<TTable>();
            for (var i = 0; i < ids.Count; i += 100)
            {
                var tempIds = ids.Skip(i).Take(100);
                var tempResult = (await Connection.ReadUncommitted().QueryAsync<TTable>($"SELECT * FROM {type.Name} WHERE {primaryKey} IN @ids", new { ids = tempIds })).AsList();
                result.AddRange(tempResult);
            }
            return result;
        }

        public async Task<List<TTable>> GetAsync(string whereAfterQueryString, object param = null)
        {
            return (await Connection.ReadUncommitted().QueryAsync<TTable>($"SELECT * FROM {typeof(TTable).Name} {FullWhereQueryCondition(whereAfterQueryString)}", param)).AsList();
        }

        public async Task<SqlMapper.GridReader> GetMutipleAsync(string querySql, object param = null, IDbTransaction tran = null)
        {
            return await Connection.ReadUncommitted().QueryMultipleAsync(querySql, param, tran);
        }

        public async Task<long> InsertAsync(TTable entity, IDbTransaction tran = null)
        {
            return await Connection.InsertAsync(entity, tran);
        }

        public async Task<bool> InsertAsync(List<TTable> entities, IDbTransaction tran = null)
        {
            return await Connection.InsertAsync<List<TTable>>(entities, tran) > 0;
        }

        public async Task<bool> UpdateAsync(TTable entity, IDbTransaction tran = null)
        {
            return await Connection.UpdateAsync(entity, tran);
        }

        public async Task<bool> UpdateAsync(List<TTable> entities, IDbTransaction tran = null)
        {
            return await Connection.UpdateAsync<List<TTable>>(entities, tran);
        }

        public async Task<bool> UpdateAsync(UpdateBuilder<TTable> builder, object param = null, IDbTransaction tran = null)
        {
            return (await Connection.ExecuteAsync(builder.BuildSql(), param, tran)) > 0;
        }

        /// <summary>
        /// Paging (just support version >= mysql 8.0)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <param name="orderByFields"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagingResult<T>> PagingAsync<T>(string querySql, string orderByFields, int pageIndex = 1, int pageSize = 15, object param = null)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 15;

            try
            {
                var result = new PagingResult<T>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                var pagingSql = string.Format(DapperContext.PAGING_SQL_SCRIPT_TEMPLATE, querySql, orderByFields, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                result.Items = (await Connection.QueryAsync<T, long, T>(pagingSql,
                    (a, b) => { result.TotalCount = b; return a; },
                    param,
                    splitOn: "TotalCount")).AsList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        private string FullWhereQueryCondition(string whereAfterQueryString)
        {
            if (string.IsNullOrEmpty(whereAfterQueryString))
                return string.Empty;
            if (whereAfterQueryString.Length > 6)
            {
                var prefix = whereAfterQueryString.Substring(0, 6).ToUpper();
                if (prefix.Equals("WHERE "))
                    return whereAfterQueryString;
            }
            return $"WHERE {whereAfterQueryString}";
        }

        public async Task<List<TRelationalTab1>> GetRelationalTablesAsync<TRelationalTab1>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(new List<TKey> { id });
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>, List<TRelationalTab7>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6, TRelationalTab7>(TKey id)
        {
            return await GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6, TRelationalTab7>(new List<TKey> { id });
        }

        public async Task<List<TRelationalTab1>> GetRelationalTablesAsync<TRelationalTab1>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return new List<TRelationalTab1>();
            }

            var sql = $@"SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            return items1;
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            return (items1, items2);
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            return (items1, items2, items3);
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            return (items1, items2, items3, items4);
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>(),
                    new List<TRelationalTab5>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab5).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TRelationalTab5>()).AsList();
            return (items1, items2, items3, items4, items5);
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>(),
                    new List<TRelationalTab5>(),
                    new List<TRelationalTab6>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab5).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab6).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TRelationalTab5>()).AsList();
            var items6 = (await reader.ReadAsync<TRelationalTab6>()).AsList();
            return (items1, items2, items3, items4, items5, items6);
        }

        public async Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>, List<TRelationalTab7>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6, TRelationalTab7>(List<TKey> ids)
        {
            var keyName = typeof(TTable).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>(),
                    new List<TRelationalTab5>(),
                    new List<TRelationalTab6>(),
                    new List<TRelationalTab7>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab5).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab6).Name} WHERE {keyName} IN @ids
                SELECT * FROM {typeof(TRelationalTab7).Name} WHERE {keyName} IN @ids
            ";
            var reader = await this.GetMutipleAsync(sql, new { ids = ids });
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TRelationalTab5>()).AsList();
            var items6 = (await reader.ReadAsync<TRelationalTab6>()).AsList();
            var items7 = (await reader.ReadAsync<TRelationalTab7>()).AsList();
            return (items1, items2, items3, items4, items5, items6, items7);
        }
    }
}
