using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCode.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCode.EventBusService
{
    public class ServiceBootstrapper:IDisposable
    {
        private IContainer _container = null;
        private ContainerBuilder builder;
        public ServiceBootstrapper()
        {
             builder=new ContainerBuilder();
            
        }

        public void Start()
        {
            _container.Resolve<ICommandBusServer>().Start();
        }

        public  IServiceProvider Build(IServiceCollection services)
        {
            builder.RegisterModule<DefaultModule>();
            _container = builder.Build();
            return new AutofacServiceProvider(_container);
        }

        public void Dispose()
        {
            _container.Dispose();
            _container = null;
        }
    }
}
