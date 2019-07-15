using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyCode.Core.Data.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// 获取实体类型信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEntityType FindEntityType(this DbContext context, Type type)
        {
            return ((DbContext)context).Model.FindEntityType(type);
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IProperty FindPrimaryKey(this DbContext context, Type type)
        {
            //找模型实体中标记为主键的属性名
            foreach (IProperty property in context.GetProperties(type))
            {
                if (property.IsPrimaryKey())
                {
                    return property;
                }
            }
            //如果没有，则返回Id
            return null;
        }

        /// <summary>
        /// 获取实体模型类型的属性信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<IProperty> GetProperties(this DbContext context, Type type)
        {
            return ((DbContext)context).Model.FindEntityType(type).GetProperties();
        }
    }
}
