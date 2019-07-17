using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace EasyCode.Core.Data.DapperExtensions
{
    internal class Session:ISession
    {
        IDbTransaction _transaction;
        public IDbConnection Connection { get; }
        public Session(IDbConnection connection)
        {
            Connection = connection;
            _transaction = null;
        }

        public PagedList<T> Paged<T>(string sql, int pageIndex, int pageSize, object param = null)
        {
            SQLParts parts;
            PagingHelper.Instance.SplitSQL(sql, out parts);
            var sqlselect = parts.Sql + $" offset {pageIndex * pageSize} row fetch next {pageSize} row only;";
            var sqlcount = parts.SqlCount;
            var reader = QueryMultiple(sqlselect + " " + sqlcount, param);
            var list = reader.Read<T>();
            var t = reader.Read<int>().FirstOrDefault();
            return new PagedList<T>(list, pageIndex, pageSize, t);
        }

        public int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Execute(sql, param, null, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            if (typeof(T) == typeof(IDictionary<string, object>))
            {
                return Connection.Query(sql, param, _transaction, true, commandTimeout, commandType).OfType<T>();
            }
            return Connection.Query<T>(sql, param, _transaction, true, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(sql, param, null, true, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(Type type, string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(type, sql, param, _transaction, true, commandTimeout, commandType);
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
             _transaction = Connection.BeginTransaction(); 
        }

        public void BeginTransaction(IsolationLevel il)
        {
            if (_transaction == null)
             _transaction = Connection.BeginTransaction(il); 
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;

        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public int Execute(CommandDefinition definition)
        {
            return Connection.Execute(definition);
        }

        public IEnumerable<T> Query<T>(CommandDefinition definition)
        {
            return Connection.Query<T>(definition);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public SqlMapper.GridReader QueryMultiple(CommandDefinition command)
        {
            return Connection.QueryMultiple(command);
        }

        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Connection.QueryMultiple(new CommandDefinition(sql, param, _transaction, commandTimeout, commandType));
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
