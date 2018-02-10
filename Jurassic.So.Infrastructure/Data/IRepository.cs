using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Data
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity, bool submit = true);

        /// <summary>附加实体</summary>
        TEntity Attach(TEntity entity, params object[] keyValues);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity, bool submit = true);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity, bool submit = true);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void DeleteList(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void DeleteList(IEnumerable<TEntity> entities);

         /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity Find(object key);

        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList<TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TKey>> orderbyExpr, int orderDirection);

        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="returnCount"></param>
        /// <returns></returns>
        IEnumerable<TResult> FindList<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int returnCount = -1);

        /// <summary>
        /// 分页查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<TResult> FindListByPage<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int pageSize, int pageNo, out int recordCount);
    
        /// <summary>
        /// 提交保存所有变更操作
        /// </summary>
        void Submit();
    }
}
