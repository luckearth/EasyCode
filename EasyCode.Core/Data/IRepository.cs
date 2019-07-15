using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EasyCode.Core.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 获取 实体IQueryable 对象 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();
        /// <summary>
        /// 根据条件获取 实体IQueryable 对象 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 直接执行 sql 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 根据主键异步获取单个对象
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] keyValues);
        /// <summary>
        /// 根据表达式查询单一实体,如有多个则返回第一个
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 根据表达式查询单一实体,如有多个则有异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 取得所有实体IList 对象
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        Task<List<TEntity>> ListAsync();

        /// <summary>
        /// 数据分页
        /// </summary>
        /// <typeparam name="PEntity"></typeparam>
        /// <param name="sortBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize);
        /// <summary>
        /// 数据分页
        /// </summary>
        /// <typeparam name="PEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sortBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="desc"></param>
        /// <returns></returns>

        PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 异步添加对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> list);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        void Update(TEntity entity);
        /// <summary>
        /// 部分字段更新
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] expressions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="entity"></param>
        void SoftDelete(TEntity entity);
        void SoftDelete(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<TEntity> entities);
    }
}
