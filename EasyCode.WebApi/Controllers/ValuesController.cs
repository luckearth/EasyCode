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
        public ActionResult<IEnumerable<string>> Get()
        {
            var items=_sessionFactory.GetSession().Query< TestEntity>("select * from Test");
            return new string[] { "value1", "value2" };
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
