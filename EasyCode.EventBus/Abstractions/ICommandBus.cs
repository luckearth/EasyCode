using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCode.EventBus.Abstractions
{
    public interface ICommandBus
    {
        Task<byte[]> GetResponse(string requestType, byte[] requestBuffer);
    }
}
