using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core.Utility.Sequence
{
    /// <summary>
    /// Twitter Snowflake 算法生成序列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdGenerator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Generates new identifier every time the method is called
        /// </summary>
        /// <returns>new identifier</returns>
        T GenerateId();
    }
}
