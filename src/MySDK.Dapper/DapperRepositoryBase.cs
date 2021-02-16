using Microsoft.Extensions.Configuration;
using MySDK.Dapper.Extentions;
using MySDK.DependencyInjection;
using System;
using System.Data;

namespace MySDK.Dapper.Sql
{
    public class DapperRepositoryBase<T> where T : IDbConnection, IDisposable
    {

        private IDbConnection _conn;
        public IDbConnection Connection
        {
            get
            {
                if (_conn == null)
                    throw new NullReferenceException("The connection object don't initalize or disposed");

                if (_conn.State != ConnectionState.Open)
                {
                    _conn.Open();
                }
                return _conn;
            }
        }

        public DapperRepositoryBase(string connectionName)
        {
            var connectionString = MyServiceProvider.Configuration.GetConnectionString(connectionName);
            _conn = GetDbConnection(connectionString);
        }

        public IDbConnection GetDbConnection(string connectionString)
        {
            return connectionString.GetDbConnection<T>();
        }

        public void Dispose()
        {
            if (_conn != null)
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
                _conn.Dispose();
                _conn = null;
            }
        }
    }

    
}
