using BackendDBAstafiev.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackendDBAstafiev.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        // GET: api/database
        [HttpGet]
        public Database GetDatabase()
        {
            var database = Database.Load("database.txt");
            return database;
        }

        // POST api/database
        [HttpPost]
        public void CreateDatabase([FromBody] string dbName)
        {
            var database = new Database(dbName);
            database.Save("database.txt");
        }

        // PUT api/database/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/database/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
