using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCode.EventBus.Abstractions
{
    public interface ICommandBusClient
    {
        Task<byte[]> GetResponseAsync(string requestType, byte[] requestBuffer);
    }
}
