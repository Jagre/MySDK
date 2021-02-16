using Dapper;
using Dapper.Contrib.Extensions;
using MySDK.Basic.Models;
using MySDK.Dapper.Extentions;
using MySDK.Dapper.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySDK.Dapper
{
    public class MySqlDapperRepository<TTable, TKey> : DapperRepositoryBase<MySqlConnection>, IDapperRepository<TTable, TKey>, IDisposable
        where TTable : class
        where TKey : struct
    {
        private readonly string _connectionString;

        //public MySqlDapperRepository(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        //public MySqlDapperRepository(IConfiguration config, string connectionName)
        //{
        //    _connectionString = config.GetConnectionString(connectionName);
        //}

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
            return await Connection.ExecuteAsync($"DELETE {typeof(TTable).Name} {FullWhereQueryCondition(whereAfterQueryString)}", param, tran) > 0;
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

        public async Task<PagingResult<T>> PagingAsync<T>(string querySql, string orderBy, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            try
            {
                var pagingSql = string.Format(DapperExecuter.PAGING_SQL_SCRIPT_TEMPLATE, querySql, orderBy, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
                var items = (await Connection.QueryAsync<T>(pagingSql)).AsList();
                return new PagingResult<T>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    //TotalCount =  //TODO Jagre
                    Items = items
                };
            }
            catch (Exception ex)
            {
                throw ex;
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

    }
}
