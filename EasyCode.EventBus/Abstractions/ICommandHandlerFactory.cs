using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.EventBus.Abstractions
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler CreateCommandHandler(string commandType);
    }
}
