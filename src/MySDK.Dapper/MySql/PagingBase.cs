using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MySDK.Dapper
{
    public class PagingBase
    {
        public async Task<T> PagingAsync<T>()
        {
            return await Task.FromResult<T>(default(T));
        }
    }
}
