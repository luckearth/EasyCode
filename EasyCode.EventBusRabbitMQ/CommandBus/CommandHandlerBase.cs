using EasyCode.EventBus.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EasyCode.EventBusRabbitMQ.CommandBus
{
    public abstract class CommandHandlerBase<TRequest, TResponse> : ICommandHandler
    {
        public async Task<byte[]> HandleAsync(byte[] requestBuffer)
        {
            JsonSerializer serializer = new JsonSerializer();
            byte[] responseBuffer;
            using(var stream=new MemoryStream(requestBuffer))
            {
                using (var requestString = new StreamReader(stream))
                {
                    using (var reader = new JsonTextReader(requestString))
                    {
                        TRequest request = serializer.Deserialize<TRequest>(reader);
                        TResponse response = await Handle(request);
                        using(MemoryStream memoryStream=new MemoryStream())
                        {
                            StreamWriter writer = new StreamWriter(memoryStream);
                            serializer.Serialize(writer, response);
                            memoryStream.Flush();
                            responseBuffer = memoryStream.ToArray();
                        }
                    }
                }
            }
            return responseBuffer;
        }
        public abstract Task<TResponse> Handle(TRequest request);
    }
}
