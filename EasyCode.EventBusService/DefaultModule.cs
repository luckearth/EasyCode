using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using EasyCode.EventBus.Abstractions;
using EasyCode.EventBusRabbitMQ;
using EasyCode.EventBusRabbitMQ.CommandBus;
using EasyCode.EventBusService.Handlers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
namespace EasyCode.EventBusService
{
    public class DefaultModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterAssemblyTypes(typeof(ProductHandler).Assembly)
                .Where(a => a.IsAssignableTo<ICommandHandler>())
                .Named<ICommandHandler>(t => t.Name.Replace("Handler", ""))
                .InstancePerLifetimeScope();
            builder.Register<Func<string, ICommandHandler>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return (name) => context.ResolveNamed<ICommandHandler>(name);
            });
            builder.Register<DefaultRabbitMQPersistentConnection>(sp =>
            {
                //var logger = sp.Resolve<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = "192.168.3.12",
                    
                };
                factory.Port = 5672;
                factory.UserName = "luckearth";
                factory.Password = "dfs123";
                var retryCount = 5;
                return new DefaultRabbitMQPersistentConnection(factory,  retryCount);
            }).As<IRabbitMQPersistentConnection>().SingleInstance();
            builder.RegisterType<CommandHandlerFactory>().As<ICommandHandlerFactory>().SingleInstance();
            builder.RegisterType<RabbitMqCommandBusServer>().As<ICommandBusServer>().SingleInstance();
        }
    }
}
