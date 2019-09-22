using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyCode.EventBusRabbitMQ.CommandBus;
using EasyCode.EventBusService.Models;

namespace EasyCode.EventBusService.Handlers
{
    public class ProductHandler:CommandHandlerBase<ProductRequest,ProductResponse>
    {
        public override Task<ProductResponse> Handle(ProductRequest request)
        {
            ProductResponse response=new ProductResponse();
            response.Id = 10;
            response.ProductName = "新产品上线";
            response.Price = 200.1;
            return Task.FromResult(response);
        }
    }
}
