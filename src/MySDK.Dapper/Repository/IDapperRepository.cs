using Dapper;
using MySDK.Basic.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MySDK.Dapper
{
    public interface IDapperRepository<TTable, TKey>
        where TTable : class
        where TKey : struct
    {
        Task<TTable> GetAsync(TKey id);

        Task<List<TTable>> GetAsync(List<TKey> ids);

        Task<List<TTable>> GetAsync(string whereAfterQueryString, object param = null);

        Task<SqlMapper.GridReader> GetMutipleAsync(string querySql, object param = null, IDbTransaction tran = null);

        Task<List<TRelationalTab1>> GetRelationalTablesAsync<TRelationalTab1>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(TKey id);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>, List<TRelationalTab7>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6, TRelationalTab7>(TKey id);

        Task<List<TRelationalTab1>> GetRelationalTablesAsync<TRelationalTab1>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6>(List<TKey> ids);

        Task<(List<TRelationalTab1>, List<TRelationalTab2>, List<TRelationalTab3>, List<TRelationalTab4>, List<TRelationalTab5>, List<TRelationalTab6>, List<TRelationalTab7>)> GetRelationalTablesAsync<TRelationalTab1, TRelationalTab2, TRelationalTab3, TRelationalTab4, TRelationalTab5, TRelationalTab6, TRelationalTab7>(List<TKey> ids);

        Task<bool> UpdateAsync(TTable entity, IDbTransaction tran = null);

        Task<bool> UpdateAsync(List<TTable> entities, IDbTransaction tran = null);

        Task<bool> UpdateAsync(UpdateBuilder<TTable> builder, object param = null, IDbTransaction tran = null);

        Task<bool> DeleteAsync(TTable entity, IDbTransaction tran = null);

        Task<bool> DeleteAsync(List<TTable> entities, IDbTransaction tran = null);

        Task<bool> DeleteAsync(string whereAfterQueryString, object param = null, IDbTransaction tran = null);

        Task<long> InsertAsync(TTable entity, IDbTransaction tran = null);

        Task<bool> InsertAsync(List<TTable> entities, IDbTransaction tran = null);

        Task<PagingResult<T>> PagingAsync<T>(string querySql, string orderByFields, int pageIndex = 1, int pageSize = 15, object param = null);
    }
}
