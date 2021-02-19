using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySDK.Dapper.Extensions
{
    public static class RepositoryExtension
    {
        public static async Task<List<TAssociativeTab>> GetAssociativeTables<TTab, TKey, TAssociativeTab>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab>(new List<TKey> { id });
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2>(new List<TKey> { id });
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3>(new List<TKey> { id });
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4>(new List<TKey> { id });
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>, List<TAssociativeTab5>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5>(new List<TKey> { id });
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>, List<TAssociativeTab5>, List<TAssociativeTab6>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5, TAssociativeTab6>(this IDapperRepository<TTab, TKey> repo, TKey id)
            where TTab : class
            where TKey : struct
        {
            return await repo.GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5, TAssociativeTab6>(new List<TKey> { id });
        }

        public static async Task<List<TAssociativeTab>> GetAssociativeTables<TTab, TKey, TAssociativeTab>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
                return new List<TAssociativeTab>();

            var sql = $@"SELECT * From {typeof(TAssociativeTab).Name} WHERE {keyName} in @ids";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items = (await reader.ReadAsync<TAssociativeTab>()).AsList();
            return items;
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TAssociativeTab1>(),
                    new List<TAssociativeTab2>());
            }

            var sql = $@"
                SELECT * From {typeof(TAssociativeTab1).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab2).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items1 = (await reader.ReadAsync<TAssociativeTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TAssociativeTab2>()).AsList();
            return (items1, items2);
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TAssociativeTab1>(),
                    new List<TAssociativeTab2>(),
                    new List<TAssociativeTab3>());
            }

            var sql = $@"
                SELECT * From {typeof(TAssociativeTab1).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab2).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab3).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items1 = (await reader.ReadAsync<TAssociativeTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TAssociativeTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TAssociativeTab3>()).AsList();
            return (items1, items2, items3);
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TAssociativeTab1>(),
                    new List<TAssociativeTab2>(),
                    new List<TAssociativeTab3>(),
                    new List<TAssociativeTab4>());
            }

            var sql = $@"
                SELECT * From {typeof(TAssociativeTab1).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab2).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab3).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab4).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items1 = (await reader.ReadAsync<TAssociativeTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TAssociativeTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TAssociativeTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TAssociativeTab4>()).AsList();
            return (items1, items2, items3, items4);
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>, List<TAssociativeTab5>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TAssociativeTab1>(),
                    new List<TAssociativeTab2>(),
                    new List<TAssociativeTab3>(),
                    new List<TAssociativeTab4>(),
                    new List<TAssociativeTab5>());
            }

            var sql = $@"
                SELECT * From {typeof(TAssociativeTab1).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab2).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab3).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab4).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab5).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items1 = (await reader.ReadAsync<TAssociativeTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TAssociativeTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TAssociativeTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TAssociativeTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TAssociativeTab5>()).AsList();
            return (items1, items2, items3, items4, items5);
        }

        public static async Task<(List<TAssociativeTab1>, List<TAssociativeTab2>, List<TAssociativeTab3>, List<TAssociativeTab4>, List<TAssociativeTab5>, List<TAssociativeTab6>)> GetAssociativeTables<TTab, TKey, TAssociativeTab1, TAssociativeTab2, TAssociativeTab3, TAssociativeTab4, TAssociativeTab5, TAssociativeTab6>(this IDapperRepository<TTab, TKey> repo, IEnumerable<TKey> ids)
            where TTab : class
            where TKey : struct
        {
            var keyName = typeof(TTab).GetPrimaryKeyName();
            if (string.IsNullOrEmpty(keyName))
            {
                return (new List<TAssociativeTab1>(),
                    new List<TAssociativeTab2>(),
                    new List<TAssociativeTab3>(),
                    new List<TAssociativeTab4>(),
                    new List<TAssociativeTab5>(),
                    new List<TAssociativeTab6>());
            }

            var sql = $@"
                SELECT * From {typeof(TAssociativeTab1).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab2).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab3).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab4).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab5).Name} WHERE {keyName} in @ids
                SELECT * From {typeof(TAssociativeTab6).Name} WHERE {keyName} in @ids
            ";
            var reader = await repo.GetMutipleAsync(sql, ids);
            var items1 = (await reader.ReadAsync<TAssociativeTab1>()).AsList();
            var items2 = (await reader.ReadAsync<TAssociativeTab2>()).AsList();
            var items3 = (await reader.ReadAsync<TAssociativeTab3>()).AsList();
            var items4 = (await reader.ReadAsync<TAssociativeTab4>()).AsList();
            var items5 = (await reader.ReadAsync<TAssociativeTab5>()).AsList();
            var items6 = (await reader.ReadAsync<TAssociativeTab6>()).AsList();
            return (items1, items2, items3, items4, items5, items6);
        }
    }
}
