using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace EasyCode.Core.Data.DapperExtensions
{
    public interface ISession: IDisposable
    {
        PagedList<T> Paged<T>(string sql, int pageIndex, int pageSize, object param = null);
        int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<dynamic> Query(System.Type type, string sql, object param = null, System.Data.CommandType? commandType = null, int? commandTimeout = 0);
        void BeginTransaction();
        void BeginTransaction(System.Data.IsolationLevel il);
        void CommitTransaction();
        void RollbackTransaction();

        IDbConnection Connection { get; }

        int Execute(CommandDefinition definition);

        IEnumerable<T> Query<T>(CommandDefinition definition);

        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);

        SqlMapper.GridReader QueryMultiple(CommandDefinition command);
        SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, System.Data.CommandType? commandType = null, int? commandTimeout = 0);
    }
}
