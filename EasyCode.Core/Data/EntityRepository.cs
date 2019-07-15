using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EasyCode.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EasyCode.Core.Data
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        /// <summary>
        /// 数据库上下文抽象
        /// </summary>
        private DbContext _context;
        /// <summary>
        /// 构造函数，注入需要的接口
        /// </summary>
        /// <param name="mainContext">数据</param>
        /// <param name="logger"></param>
        public EntityRepository(DbContext mainContext)
        {
            _context = mainContext;

        }
        /// <summary>
        /// 数据库表实体
        /// </summary>
        private DbSet<TEntity> Entities => _context.Set<TEntity>();
        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity) => await Entities.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> list) => await Entities.AddRangeAsync(list);

        public void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }
        public void SoftDelete(TEntity entity)
        {
            Entities.Remove(entity);
            OnBeforeSaving();
        }
        public void SoftDelete(Expression<Func<TEntity, bool>> predicate)
        {
            IList<TEntity> list = Entities.Where(predicate).ToList();
            Entities.RemoveRange(list);
            OnBeforeSaving();
        }
        private void OnBeforeSaving()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDelete"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDelete"] = true;
                        break;
                }
            }
        }
        public void Delete(object id)
        {
            var property = _context.FindPrimaryKey(typeof(TEntity));
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.PropertyInfo.SetValue(entity, id);
                ((DbContext)_context).Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = Entities.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }
        public void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            IList<TEntity> list = Entities.Where(predicate).ToList();
            Entities.RemoveRange(list);

        }
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) => await Entities.AnyAsync(predicate);

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Entities.FindAsync(keyValues);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {

            return Entities.FromSql(sql, parameters);
        }

        public async Task<List<TEntity>> ListAsync() => await Entities.AsNoTracking().ToListAsync();

        public PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize)
        {
            return Query().OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize);
        }
        public PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true)
        {
            if (desc)
                return Query(predicate).OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize);
            return Query(predicate).OrderBy(sortBy).ToPagedList(pageIndex, pageSize);
        }
        public IQueryable<TEntity> Query()
        {
            return Entities.AsNoTracking();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.AsNoTracking().Where(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate) => await Entities.SingleOrDefaultAsync(predicate);

        public TEntity Update(TEntity entity, params Expression<Func<TEntity, object>>[] expressions)
        {
            Entities.Attach(entity);
            var dbContext = _context as DbContext;
            //标记更改过的属性
            foreach (var expression in expressions)
            {
                var p = dbContext?.Entry(entity).Property(expression);
                p.IsModified = true;
            }
            return entity;
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
