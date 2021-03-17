using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySDK.Dapper.Extensions
{
    public static class RepositoryExtension
    {
        public static async Task<(List<TTab>, List<TRelationalTab>)> GetRelationalTables<TTab, TKey, TRelationalTab>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(new List<TKey> { id });
        }

        public static async Task<(List<TTab>, List<TRelationalTab>)> GetRelationalTables<TTab, TKey, TRelationalTab>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
                return (new List<TTab>(), 
                    new List<TRelationalTab>());

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab>()).AsList();
            return (items, items1);
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TTab>(), 
                    new List<TRelationalTab1>(),
                    new List<TRelationalTab2>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            return (items, items1, items2);
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TTab>(), 
                    new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            return (items, items1, items2, items3);
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TTab>(), 
                    new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            return (items, items1, items2, items3, items4);
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TTab>(), 
                    new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>(),
                    new List<TRelationalTab5>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab5).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TRelationalTab5>()).AsList();
            return (items, items1, items2, items3, items4, items5);
        }

        public static async Task<(List<TTab>, List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTables<TTab, TKey, TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TTab>(), 
                    new List<TRelationalTab1>(),
                    new List<TRelationalTab2>(),
                    new List<TRelationalTab3>(),
                    new List<TRelationalTab4>(),
                    new List<TRelationalTab5>(),
                    new List<TRelationalTab6>());
            }

            var sql = $@"
                SELECT * FROM {typeof(TTab).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab1).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab2).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab3).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab4).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab5).Name} WHERE {keyName} in @ids
                SELECT * FROM {typeof(TRelationalTab6).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids); 
            var items = (await reader.ReadAsync<TTab>()).AsList();
            var items1 = (await reader.ReadAsync<TRelationalTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TRelationalTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TRelationalTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TRelationalTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TRelationalTab5>()).AsList();
            var items6 = (await reader.ReadAsync<TRelationalTab6>()).AsList();
            return (items, items1, items2, items3, items4, items5, items6);
        }
    }
}
