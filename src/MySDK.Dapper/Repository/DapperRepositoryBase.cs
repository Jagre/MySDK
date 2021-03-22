using MySDK.Dapper.Extensions;
using MySDK.DependencyInjection;
using System;
using System.Data;
using MySDK.Configuration;

namespace MySDK.Dapper
{
    public class DapperRepositoryBase<T> : IDisposable where T : IDbConnection
    {
        private IDbConnection _conn;
        public IDbConnection Connection
        {
            get
            {
                if (_conn == null)
                    throw new NullReferenceException("The connection object was uninitialized or disposed");

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
