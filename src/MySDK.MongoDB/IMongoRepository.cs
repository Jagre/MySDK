using MySDK.Basic.Models;
using MySDK.MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MySDK.MongoDB
{
    public interface IMongoRepository<T> where T : MongoEntityBase
    {
        Task<T> GetAsync(string id);

        Task<IList<T>> GetAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Insert a doc to mongo db's collection
        /// </summary>
        /// <param name="doc">needn't set value of the field Id</param>
        /// <returns>document's Id has value</returns>
        Task<T> InsertAsync(T doc);

        /// <summary>
        /// Insert a doc to mongo db's collection
        /// </summary>
        /// <param name="docs">needn't set value of the field Id</param>
        /// <returns>document's Id has value</returns>
        Task<IList<T>> InsertAsync(IEnumerable<T> docs);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter);

        Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> values);

        Task<PagingResult<T>> PagingAsync(Expression<Func<T, bool>> filter, Expression<Func<IEnumerable<T>, dynamic>> orderBy, int pageIndex = 1, int pageSize = 20);
    }
}