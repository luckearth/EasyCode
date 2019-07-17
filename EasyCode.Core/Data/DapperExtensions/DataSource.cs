using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EasyCode.Core.Data.DapperExtensions
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DataSource
    {
        public Func<IDbConnection> Source { get; set; }
        public string Name { get; set; } = "DefaultSource";
    }
}
