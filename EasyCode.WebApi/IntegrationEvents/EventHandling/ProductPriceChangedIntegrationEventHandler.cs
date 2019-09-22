using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.EventBus.Abstractions;
using EasyCode.WebApi.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;

namespace EasyCode.WebApi.IntegrationEvents.EventHandling
{
    public class ProductPriceChangedIntegrationEventHandler:IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        private ILogger _logger;
        public ProductPriceChangedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            _logger.LogInformation("我执行了价格更改");
            Console.WriteLine("价格更改了");
            await Task.CompletedTask;
        }
    }
}
