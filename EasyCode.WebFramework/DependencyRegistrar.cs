using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using EasyCode.Core.Configuration;
using EasyCode.Core.Infrastructure;
using EasyCode.IService;
using EasyCode.Service;

namespace EasyCode.WebFramework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, LiteConfig config)
        {
            builder.RegisterType<TokenProviderService>().As<ITokenProviderService>().InstancePerLifetimeScope();


        }
    }
}
