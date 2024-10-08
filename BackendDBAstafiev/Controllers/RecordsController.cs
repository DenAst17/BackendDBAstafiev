using BackendDBAstafiev.Classes;
using BackendDBAstafiev.Classes.Dto;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Column = BackendDBAstafiev.Classes.Column;
using Table = BackendDBAstafiev.Classes.Table;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendDBAstafiev.Controllers
{
    [Route("api/records")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        // GET: api/records
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/records/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/records
        [HttpPost]
        public Table? AddRecord([FromBody] SetRecordDto dto)
        {
            var database = Database.Load("database.txt");
            var selectedTable = database.Tables.FirstOrDefault(t => t.Name == dto.tableName);
            Record currentRecord = new Record();
            if (selectedTable == null || selectedTable.Columns.Count != dto.fields.Length)
            {
                return null;
            }
            else
            {
                for (var i = 0; i < dto.fields.Length; i++)
                {
                    Column column = selectedTable.Columns[i];
                    Field field = new Field(column, dto.fields[i]);
                    if (!field.Validate())
                    {
                        return null;
                    }
                    currentRecord.AddField(field);
                }
                selectedTable.AddRecord(currentRecord);
            }
            database.Save("database.txt");
            return selectedTable;
        }

        // PUT api/records/5
        [HttpPut("{recordNumber}")]
        public Table? UpdateRecord(int recordNumber, [FromBody] SetRecordDto dto)
        {
            var database = Database.Load("database.txt");
            var selectedTable = database.Tables.FirstOrDefault(t => t.Name == dto.tableName);
            if (selectedTable == null || selectedTable.Columns.Count != dto.fields.Length)
            {
                return null;
            }
            else
            {
                try
                {
                    var currentRecord = selectedTable.Records.ElementAt(recordNumber);
                    for (var i = 0; i < dto.fields.Length; i++)
                    {
                        Column column = selectedTable.Columns[i];
                        Field field = new Field(column, dto.fields[i]);
                        if (!field.Validate())
                        {
                            return null;
                        }
                        currentRecord.Fields[i] = field;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            database.Save("database.txt");
            return selectedTable;
        }


        // DELETE api/records
        [HttpDelete("{recordNumber}")]
        public Table? Delete(int recordNumber, [FromQuery] string tableName)
        {
            var database = Database.Load("database.txt");
            var selectedTable = database.Tables.FirstOrDefault(t => t.Name == tableName);
            if (selectedTable == null)
            {
                return null;
            }
            else
            {
                selectedTable.Records.RemoveAt(recordNumber);
            }
            database.Save("database.txt");
            return selectedTable;
        }
    }
}
