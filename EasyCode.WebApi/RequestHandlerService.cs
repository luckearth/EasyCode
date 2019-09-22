using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EasyCode.WebApi
{
    public class RequestHandlerService
    {
        private ICommandBusClient _client;
        private ILogger _logger;
        public RequestHandlerService(ICommandBusClient client,ILogger<RequestHandlerService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<TResponse> GetResponseAsync<TRequest, TResponse>(string requestType, TRequest request) where TResponse:new()
        {
            JsonSerializer serializer=new JsonSerializer();
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    serializer.Serialize(sw, request);
                    sw.Flush();
                    var requestBuffer = ms.ToArray();
                    try
                    {
                        var responseBuffer = await _client.GetResponseAsync(requestType, requestBuffer);
                        using (MemoryStream msb = new MemoryStream(responseBuffer))
                        {
                            StreamReader reader = new StreamReader(msb);
                            TResponse res= (TResponse)serializer.Deserialize(reader, typeof(TResponse));
                            return res;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{e.Message},{e.Source}");
                    }
                    
                }
                
            }

            return default(TResponse);

        }
    }
}
