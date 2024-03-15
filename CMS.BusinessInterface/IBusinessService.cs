using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace CMS.BusinessInterface
{
    /// <summary>
    ///  provide shared methods
    /// </summary>
    public interface IBusinessService
    {
        #region Query
        /// <summary>
        /// primary key query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(int id) where T : class;
        /// <summary>
        /// primart key query async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(int id) where T : class;
        /// <summary>
        /// single table query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISugarQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// provide conditional query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISugarQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        /// <summary>
        /// paging query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PagingData<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, Object>> funcOrderBy, bool isAsc = true) where T : class;
        #endregion

        #region Add
        /// <summary>
        /// add one entity snyc version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Insert<T>(T entity) where T : class, new();

        /// <summary>
        /// add one entity async version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> InsertAsync<T>(T entity) where T : class, new();

        /// <summary>
        /// add a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        Task<bool> InsertList<T>(List<T> tList) where T : class, new();
        #endregion

        #region Update
        /// <summary>
        /// update one entity sync and commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        bool Update<T>(T entity) where T : class, new();
        /// <summary>
        /// update a entity async and commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entity) where T : class, new();
        /// <summary>
        /// update a entity list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        bool UpdateList<T>(List<T> tList) where T : class, new();
        #endregion

        #region Delete
        /// <summary>
        /// delete by pkey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pId"></param>
        /// <returns></returns>
        bool Delete<T>(object pId) where T : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pId"></param>
        /// <returns></returns>
        /// DeleteAsync
        Task<bool> DeleteAsync<T>(object pId) where T : class, new();

        /// <summary>
        /// delete using list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        void DeleteList<T>(List<T> tList) where T : class, new();
        #endregion

        #region  sql
        ISugarQueryable<T> ExcuteQuery<T>(string sql) where T : class, new();
        #endregion 

    }
}
