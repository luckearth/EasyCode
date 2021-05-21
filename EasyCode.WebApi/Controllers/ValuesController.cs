using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.Core.Data.DapperExtensions;
using EasyCode.EventBus.Abstractions;
using EasyCode.WebApi.IntegrationEvents.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCode.WebApi.Controllers
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private RequestHandlerService _handlerService;
        private ILogger _logger;
        private IEventBus _event;
        public ValuesController(ILogger<ValuesController> logger) 
        {
           
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public async Task<ProductResponse> Get()
        {
            //var items=_sessionFactory.GetSession().Query< TestEntity>("select * from Test").ToList();
            //var list=new List<TestEntity>();
            //list.Add(new TestEntity(){Id=1,Name = "Hello1",CreateTime = DateTime.Now});
            //list.Add(new TestEntity() { Id = 2, Name = "Hello2", CreateTime = DateTime.Now });
            //list.Add(new TestEntity() { Id = 3, Name = "Hello3", CreateTime = DateTime.Now });
            //list.Add(new TestEntity() { Id = 4, Name = "Hello4", CreateTime = DateTime.Now });
            //list.Add(new TestEntity() { Id = 5, Name = "Hello5", CreateTime = DateTime.Now });
            //var product = new ProductPriceChangedIntegrationEvent(productId: 1, newPrice: 20, 36);
            //_event.Publish(product);
            //Console.WriteLine("我开始发送了");
            ProductRequest product = new ProductRequest();
            product.Id = 1;
            product.ProductName = "ss";
            try
            {
                //var response = await _handlerService.GetResponseAsync<ProductRequest, ProductResponse>("Product", product);
                //Console.WriteLine("我已向队列发送数据");
                return (new ProductResponse());
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString());
            }

            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task Get(int id)
        {
            ProductRequest product=new ProductRequest();
            var response = await _handlerService.GetResponseAsync<ProductRequest, ProductResponse>("Product", product);
            Console.WriteLine("我向队列发送数据");
            //return Json(response);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
