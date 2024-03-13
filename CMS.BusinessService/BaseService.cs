using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.BusinessInterface;
using SqlSugar;
namespace CMS.BusinessService
{
    public abstract class BaseService : IBusinessService
    {


        private ISqlSugarClient _client;
        public BaseService(ISqlSugarClient client)
        {
            _client = client;
        }

        #region Query

        public T Find<T>(int id) where T : class
        {
            return _client.Queryable<T>().InSingle(id);
        }

        public async Task<T> FindAsync<T>(int id) where T : class
        {
            return await _client.Queryable<T>().InSingleAsync(id);
        }

        public ISugarQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return _client.Queryable<T>().Where(funcWhere);
        }

        public PagingData<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, object>> funcOrderBy, bool isAsc = true) where T : class
        {
            var list = _client.Queryable<T>();
            if (funcWhere != null)
            {
                list = list.Where(funcWhere);
            }
            list = list.OrderByIF(true, funcOrderBy, isAsc ? OrderByType.Asc : OrderByType.Desc);
            return new PagingData<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                DataList = list.ToPageList(pageIndex, pageSize),
                RecordCount = list.Count()

            };

        }

        public ISugarQueryable<T> Set<T>() where T : class
        {
            return _client.Queryable<T>();
        }
        #endregion

        #region Delete
        public bool Delete<T>(object pId) where T : class, new()
        {
            T t = _client.Queryable<T>().InSingle(pId);
            return _client.Deleteable(t).ExecuteCommandHasChange();
        }

        public async Task<bool> DeleteAsync<T>(object pId) where T : class, new()
        {
            T t = _client.Queryable<T>().InSingle(pId);
            return await _client.Deleteable(t).ExecuteCommandHasChangeAsync();
        }

        public void DeleteList<T>(List<T> tList) where T : class, new()
        {
            _client.Deleteable<T>(tList).ExecuteCommandHasChange();
        }

        #endregion

        #region Update

        public void Update<T>(T entity) where T : class, new()
        {
            _client.Updateable<T>(entity).ExecuteCommand();
        }

        public async Task UpdateAsync<T>(T entity) where T : class, new()
        {
            await _client.Updateable<T>(entity).ExecuteCommandAsync();
        }

        public void UpdateList<T>(List<T> tList) where T : class, new()
        {
            _client.Updateable<T>(tList).ExecuteCommand();
        }
        #endregion

        #region Add
        public T Insert<T>(T entity) where T : class, new()
        {
            return _client.Insertable<T>(entity).ExecuteReturnEntity();
        }

        public async Task<T> InsertAsync<T>(T entity) where T : class, new()
        {
            return await _client.Insertable<T>(entity).ExecuteReturnEntityAsync();
        }

        public async Task<bool> InsertList<T>(List<T> tList) where T : class, new()
        {
            return await _client.Insertable<T>(tList).ExecuteCommandIdentityIntoEntityAsync();
        }

        #endregion

        #region Other
        public ISugarQueryable<T> ExcuteQuery<T>(string sql) where T : class, new()
        {
            return _client.SqlQueryable<T>(sql);
        }

        public void Disposal()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
        #endregion


    }
}
