using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core.Data.DapperExtensions
{
    public interface IPagingHelper
    {
        bool SplitSQL(string sql, out SQLParts parts);
    }
}
