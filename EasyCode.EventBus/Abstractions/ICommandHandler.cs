using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCode.EventBus.Abstractions
{
    public interface ICommandHandler
    {
        Task<byte[]> HandleAsync(byte[] requestBuffer);
    }
}
