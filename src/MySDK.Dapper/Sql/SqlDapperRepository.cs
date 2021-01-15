using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using MySDK.Dapper.Extention;
using MySDK.Dapper.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MySDK.Dapper
{
    public class SqlDapperRepository<TTable, TKey> : DapperContext<SqlConnection>, IDapperRepository<TTable, TKey>, IDisposable
        where TTable : class
        where TKey : struct
    {
        private readonly string _connectionString;

        public SqlDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlDapperRepository(IConfigurationRoot config, string connectionName)
        {
            _connectionString = config.GetConnectionString(connectionName);
        }

        private IDbConnection _conn;
        public IDbConnection Connection
        {
            get
            {
                if (_conn == null)
                {
                    _conn = base.GetDbConnection(_connectionString);
                }
                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }
                return _conn;
            }
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
            return await Connection.ExecuteAsync($"delete {typeof(TTable).Name} with (rowlock) {whereAfterQueryString}", param, tran) > 0;

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
            return (await Connection.QueryAsync<TTable>($"select * from {type.Name} (nolock) where {primaryKey} in @ids", new { ids })).AsList();
        }

        public async Task<List<TTable>> GetAsync(string whereAfterQueryString, object param = null)
        {
            return (await Connection.QueryAsync<TTable>($"select * from {typeof(TTable).Name} (nolock) {whereAfterQueryString}", param)).AsList();
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

        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
        }
    }
}
