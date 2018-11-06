using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SrTwoApi.v2.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        // GET: api/ToDo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "v2 value1", "v2 value2" };
        }

        // GET: api/ToDo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ToDo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
