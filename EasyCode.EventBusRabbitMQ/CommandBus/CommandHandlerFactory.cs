using EasyCode.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.EventBusRabbitMQ.CommandBus
{
    public class CommandHandlerFactory:ICommandHandlerFactory
    {
        private ILogger _logger;
        private readonly Func<string ,ICommandHandler> _factory;
        public CommandHandlerFactory(Func<string,ICommandHandler> factory,Logger<CommandHandlerFactory> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public ICommandHandler CreateCommandHandler(string commandType)
        {
            try
            {
                return _factory(commandType);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
