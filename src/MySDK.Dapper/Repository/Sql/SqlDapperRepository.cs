//using Dapper;
//using Dapper.Contrib.Extensions;
//using MySDK.Basic.Models;
//using MySDK.Dapper.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MySDK.Dapper
//{
//    public class SqlDapperRepository<TTable, TKey> : DapperRepositoryBase<SqlConnection>, IDapperRepository<TTable, TKey>, IDisposable
//        where TTable : class
//        where TKey : struct
//    {
//        private readonly string _connectionString;

//        public SqlDapperRepository(string connectionName)
//           : base(connectionName)
//        {
//        }

//        public async Task<bool> DeleteAsync(TTable entity, IDbTransaction tran = null)
//        {
//            return await Connection.DeleteAsync(entity, tran);
//        }

//        public async Task<bool> DeleteAsync(List<TTable> entities, IDbTransaction tran = null)
//        {
//            return await Connection.DeleteAsync(entities, tran);
//        }

//        public async Task<bool> DeleteAsync(string whereAfterQueryString, object param = null, IDbTransaction tran = null)
//        {
//            return await Connection.ExecuteAsync($"DELETE {typeof(TTable).Name} WITH (ROWLOCK) {whereAfterQueryString}", param, tran) > 0;

//        }

//        public async Task<TTable> GetAsync(TKey id)
//        {
//            return await Connection.ReadUncommitted().GetAsync<TTable>(id);
//        }

//        public async Task<List<TTable>> GetAsync(List<TKey> ids)
//        {
//            if (ids == null || !ids.Any())
//                return new List<TTable>();

//            var type = typeof(TTable);
//            var primaryKey = type.GetPrimaryKeyName();

//            List<TTable> result = new List<TTable>();
//            for (var i = 0; i < ids.Count; i += 100)
//            {
//                var tempIds = ids.Skip(i).Take(100);
//                var tempResult = (await Connection.QueryAsync<TTable>($"SELECT * FROM {type.Name} (NOLOCK) WHERE {primaryKey} IN @ids", new { ids = tempIds })).AsList();
//                result.AddRange(tempResult);
//            }
//            return result;
//        }

//        public async Task<List<TTable>> GetAsync(string whereAfterQueryString, object param = null)
//        {
//            return (await Connection.QueryAsync<TTable>($"SELECT * FROM {typeof(TTable).Name} (NOLOCK) {whereAfterQueryString}", param)).AsList();
//        }

//        public async Task<SqlMapper.GridReader> GetMutipleAsync(string querySql, object param = null, IDbTransaction tran = null)
//        {
//            return await Connection.ReadUncommitted().QueryMultipleAsync(querySql, param, tran);
//        }

//        public async Task<long> InsertAsync(TTable entity, IDbTransaction tran = null)
//        {
//            return await Connection.InsertAsync(entity, tran);
//        }

//        public async Task<bool> InsertAsync(List<TTable> entities, IDbTransaction tran = null)
//        {
//            return await Connection.InsertAsync<List<TTable>>(entities, tran) > 0;
//        }

//        public async Task<bool> UpdateAsync(TTable entity, IDbTransaction tran = null)
//        {
//            return await Connection.UpdateAsync(entity, tran);
//        }

//        public async Task<bool> UpdateAsync(List<TTable> entities, IDbTransaction tran = null)
//        {
//            return await Connection.UpdateAsync<List<TTable>>(entities, tran);
//        }

//        public async Task<bool> UpdateAsync(UpdateBuilder<TTable> builder, object param = null, IDbTransaction tran = null)
//        {
//            return (await Connection.ExecuteAsync(builder.BuildSql(), param, tran)) > 0;
//        }

//        public async Task<PagingResult<T>> PagingAsync<T>(string querySql, string orderByFields, int pageIndex = 1, int pageSize = 15, object param = null)
//        {
//            if (pageIndex <= 0)
//                pageIndex = 1;

//            if (pageSize <= 0)
//                pageSize = 15;

//            try
//            {
//                var result = new PagingResult<T>
//                {
//                    PageIndex = pageIndex,
//                    PageSize = pageSize
//                };
//                var pagingSql = string.Format(DapperContext.PAGING_SQL_SCRIPT_TEMPLATE, querySql, orderByFields, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
//                result.Items = (await Connection.QueryAsync<T, long, T>(pagingSql,
//                    (a, b) => { result.TotalCount = b; return a; },
//                    param,
//                    splitOn: "TotalCount")).AsList();
//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw ex.GetBaseException();
//            }
//        }

//    }
//}
