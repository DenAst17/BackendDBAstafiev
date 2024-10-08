using BackendDBAstafiev.Classes;
using BackendDBAstafiev.Classes.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendDBAstafiev.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        // GET api/tables/name
        [HttpGet("{name}")]
        public Table? Get(string name)
        {
            var database = Database.Load("database.txt");
            var table = database.Tables.Find(x => x.Name == name);
            return table;
        }

        // GET api/tables/join
        [HttpPost("join")]
        public Table? JoinTables([FromBody] JoinTablesDto dto)
        {
            var database = Database.Load("database.txt");

            var table1 = database.Tables.FirstOrDefault(t => t.Name == dto.joinTable1);
            var table2 = database.Tables.FirstOrDefault(t => t.Name == dto.joinTable2);

            var column1 = table1?.Columns.FirstOrDefault(c => c.Name == dto.joinColumn1);
            var column2 = table2?.Columns.FirstOrDefault(c => c.Name == dto.joinColumn2);

            var resultTable = table1?.Join(table2, column1, column2);

            if (resultTable != null)
            {
                database.AddTable(resultTable);
                database.Save("database.txt");
            }
            return resultTable;
        }

        // POST api/tables
        [HttpPost]
        public void AddTable([FromBody] string name)
        {
            var database = Database.Load("database.txt");
            var newTable = new Table(name);
            database.AddTable(newTable);
            database.Save("database.txt");
        }

        // PUT api/tables/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/tables/5
        [HttpDelete("{name}")]
        public void DeleteTable(string name)
        {
            var database = Database.Load("database.txt");
            database.RemoveTable(name);
            database.Save("database.txt");
        }
    }
}
