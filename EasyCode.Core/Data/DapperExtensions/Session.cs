using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace EasyCode.Core.Data.DapperExtensions
{
    internal class Session:ISession
    {
        public IDbConnection Connection { get; }
        public Session(IDbConnection connection)
        {
            Connection = connection;
        }

        public int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> Query(Type type, string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public int Execute(CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(CommandDefinition definition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }

        public SqlMapper.GridReader QueryMultiple(CommandDefinition command)
        {
            throw new NotImplementedException();
        }

        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            throw new NotImplementedException();
        }
    }
}
