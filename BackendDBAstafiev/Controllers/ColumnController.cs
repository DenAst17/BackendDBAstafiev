using BackendDBAstafiev.Classes;
using BackendDBAstafiev.Classes.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendDBAstafiev.Controllers
{
    [Route("api/columns")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        // GET: api/columns
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/columns/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/columns
        [HttpPost]
        public void AddColumn([FromBody] AddColumnDto dto)
        {
            var database = Database.Load("database.txt");
            var table = database.Tables.FirstOrDefault(t => t.Name == dto.tableName);
            var newColumn = new Column(dto.columnName, dto.dataType);
            if(table != null)
            {
                table.AddColumn(newColumn);
            }
            database.Save("database.txt");
        }

        // PUT api/columns/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/columns/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
