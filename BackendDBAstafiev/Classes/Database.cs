using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDBAstafiev.Classes
{
    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }

        public Database(string name)
        {
            Name = name;
            Tables = new List<Table>();
        }

        public void AddTable(Table table)
        {
            Tables.Add(table);
        }

        public void RemoveTable(string tableName)
        {
            Tables.RemoveAll(t => t.Name == tableName);
        }

        public void Save(string filePath)
        {
            // Save database and its tables to a file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Database: {Name}");
                foreach (var table in Tables)
                {
                    writer.WriteLine($"Table: {table.Name}");
                    table.Save(writer);
                    writer.WriteLine("TableEnd");
                }
            }
        }

        public static Database Load(string filePath)
        {
            // Load the database from a file
            Database db = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string dbName = reader.ReadLine()?.Replace("Database: ", "");
                if (dbName != null)
                {
                    db = new Database(dbName);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Table:"))
                        {
                            string tableName = line.Replace("Table: ", "");
                            Table table = Table.Load(reader, tableName);
                            db.AddTable(table);
                        }
                    }
                }
            }
            return db;
        }
    }
}
