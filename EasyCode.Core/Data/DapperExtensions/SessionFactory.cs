using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core.Data.DapperExtensions
{
    public class SessionFactory:ISessionFactory
    {
        private static readonly ConcurrentDictionary<string, DataSource> source=new ConcurrentDictionary<string, DataSource>();

        public SessionFactory()
        {

        }
        public static void AddDataSource(DataSource dataSource)
        {
            source[dataSource.Name]=dataSource;
        }
        public ISession GetSession(string name = "DefaultSource")
        {
            ISession session = null;
            var ds = source[name];
            if(ds!=null)
                session=new Session(ds.Source());
            return session;
        }
    }
}
