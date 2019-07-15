using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core
{
    /// <summary>
    /// 数据分页接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<out T> : IEnumerable<T>, IPagedList
    {

    }

    public interface IPagedList : IEnumerable
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 每页数据量
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 数据总量
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 分页总数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
