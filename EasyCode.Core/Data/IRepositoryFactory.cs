using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core.Data
{
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 仓储工厂接口
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
