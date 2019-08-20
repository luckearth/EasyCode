using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCode.Core.Data.DapperExtensions;
using Microsoft.AspNetCore.Mvc;

namespace EasyCode.WebApi.Controllers
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ISessionFactory _sessionFactory;
        public ValuesController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<List<TestEntity>> Get()
        {
            var items=_sessionFactory.GetSession().Query< TestEntity>("select * from Test").ToList();
            var list=new List<TestEntity>();
            list.Add(new TestEntity(){Id=1,Name = "Hello1",CreateTime = DateTime.Now});
            list.Add(new TestEntity() { Id = 2, Name = "Hello2", CreateTime = DateTime.Now });
            list.Add(new TestEntity() { Id = 3, Name = "Hello3", CreateTime = DateTime.Now });
            list.Add(new TestEntity() { Id = 4, Name = "Hello4", CreateTime = DateTime.Now });
            list.Add(new TestEntity() { Id = 5, Name = "Hello5", CreateTime = DateTime.Now });
            return items;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
