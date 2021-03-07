using MySDK.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySDK.Basic.Models;

namespace MySDK.MongoDB
{
    public class MongoRepository<T> : MongoDbContext, IMongoRepository<T> where T : MongoEntityBase
    {
        public IMongoCollection<T> Collection { get; private set; }

        public MongoRepository(string connectionName)
            : base(connectionName)
        {
            Collection = GetCollection<T>();
        }

        public async Task<T> GetAsync(string id)
        {
            var result = await Collection.FindAsync(i => i.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<T> InsertAsync(T doc)
        {
            await Collection.InsertOneAsync(doc);
            return doc;
        }

        public async Task<IList<T>> InsertAsync(IEnumerable<T> docs)
        {
            await Collection.InsertManyAsync(docs);
            return docs.ToList();
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            var result = await Collection.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<IList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            var items = await Collection.FindAsync(filter);
            return await items.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> values)
        {
            if (values == null)
                return false;

            var updates = new List<UpdateDefinition<T>>();
            var updateBuilder = Builders<T>.Update;
            foreach (var v in values)
            {
                //it need to special process on the field type is byte or bool
                //if (v.Value is byte)
                //{
                //    updates.Add(updateBuilder.Set(v.Key, (byte)v.Value));
                //}
                //else if (v.Value is bool)
                //{
                //    updates.Add(updateBuilder.Set(v.Key, (bool)v.Value));
                //}
                //else
                //{
                updates.Add(updateBuilder.Set(v.Key, v.Value));
                //}
            }

            var result = await Collection.UpdateManyAsync(filter, updateBuilder.Combine(updates));
            return result.ModifiedCount > 0;
        }

        public async Task<PagingResult<T>> PagingAsync(Expression<Func<T, bool>> filter, Expression<Func<IEnumerable<T>, dynamic>> orderBy, int pageIndex = 1, int pageSize = 20)
        {
            var result = new PagingResult<T> { PageIndex = pageIndex, PageSize = pageSize };
            result.TotalCount = await this.Collection.CountDocumentsAsync(filter);
            if (result.TotalCount == 0)
                return result;

            if (orderBy == null)
                orderBy = (i) => i.OrderBy(j => j.Id);
            //T1: query type; T2: return type (select f1, f2, f3 ...)
            result.Items = await (await this.Collection.FindAsync(filter, new FindOptions<T, T>
            {
                Skip = (pageIndex - 1) * pageSize,
                Limit = pageSize,
                Sort = GetSortDefinition(orderBy),
            })).ToListAsync();
            return result;
        }



        public SortDefinition<TSort> GetSortDefinition<TSort>(Expression<Func<IEnumerable<TSort>, dynamic>> orderBy)
        {
            var stackMember = new Stack<string>();
            var stackSortMethod = new Stack<int>();
            new SortExpressionParser().Parsing<TSort>(orderBy, stackMember, stackSortMethod);
            if (stackMember.Any() && stackSortMethod.Any() && stackMember.Count == stackSortMethod.Count)
            {
                var length = stackMember.Count;
                SortDefinitionBuilder<TSort> builder = new SortDefinitionBuilder<TSort>();
                List<SortDefinition<TSort>> list = new List<SortDefinition<TSort>>();
                for (var i = 0; i < length; i++)
                {
                    var name = stackMember.Pop();
                    var way = stackSortMethod.Pop();
                    if (way == 1)
                    {
                        list.Add(builder.Ascending(name));
                    }
                    else
                    {
                        list.Add(builder.Descending(name));
                    }
                }
                var definition = builder.Combine(list);
                return definition;
            }
            return null;
        }

    }
}