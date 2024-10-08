using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDBAstafiev.Classes
{
    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<Record> Records { get; set; }

        public Table(string name)
        {
            Name = name;
            Columns = new List<Column>();
            Records = new List<Record>();
        }

        public void AddColumn(Column column)
        {
            Columns.Add(column);
        }

        public void AddRecord(Record record)
        {
            if (record.Fields.Count == Columns.Count)
            {
                Records.Add(record);
            }
            else
            {
                throw new Exception("Record does not match table schema.");
            }
        }

        public void Save(StreamWriter writer)
        {
            writer.WriteLine("Columns:");
            foreach (var column in Columns)
            {
                writer.WriteLine($"Column: {column.Name}, Type: {column.DataType}");
            }

            writer.WriteLine("Records:");
            foreach (var record in Records)
            {
                record.Save(writer);
            }
        }

        public static Table Load(StreamReader reader, string tableName)
        {
            Table table = new Table(tableName);
            string line;
            while ((line = reader.ReadLine()) != null && !line.StartsWith("TableEnd"))
            {
                if (line.StartsWith("Column:"))
                {
                    string[] parts = line.Split(',');
                    string columnName = parts[0].Replace("Column: ", "");
                    string columnType = parts[1].Replace(" Type: ", "");
                    table.AddColumn(new Column(columnName, columnType));
                }
                else if (line.StartsWith("Record:"))
                {
                    Record record = Record.Load(line, table.Columns);
                    while(record.Fields.Count < table.Columns.Count)
                    {
                        record.AddField(new Field(table.Columns[record.Fields.Count], ""));
                    }
                    table.AddRecord(record);
                }
            }
            return table;
        }
        public Table Join(Table otherTable, Column thisColumn, Column otherColumn)
        {
            // Validate columns match
            if (thisColumn.DataType != otherColumn.DataType)
            {
                throw new Exception("Column types do not match for join.");
            }

            // Find the index of the columns in their respective tables
            int thisColumnIndex = Columns.IndexOf(Columns.FirstOrDefault(c => c.Name == thisColumn.Name));
            int otherColumnIndex = otherTable.Columns.IndexOf(otherTable.Columns.FirstOrDefault(c => c.Name == otherColumn.Name));

            if (thisColumnIndex == -1 || otherColumnIndex == -1)
            {
                throw new Exception("Column not found in one of the tables.");
            }

            // Create result table
            Table resultTable = new Table("JoinedTable" + DateTime.Now.ToString());

            // Add columns from both tables to the result table
            foreach (var col in Columns)
            {
                resultTable.AddColumn(new Column($"{Name}.{col.Name}", col.DataType));
            }

            foreach (var col in otherTable.Columns)
            {
                resultTable.AddColumn(new Column($"{otherTable.Name}.{col.Name}", col.DataType));
            }

            // Perform the join (INNER JOIN)
            foreach (var record in Records)
            {
                var firstField = record.Fields[thisColumnIndex];

                foreach (var otherRecord in otherTable.Records)
                {
                    var secondField = otherRecord.Fields[otherColumnIndex];
                    if (firstField.Value == secondField.Value)
                    {
                        // Create a deep copy of fields to avoid modifying original records
                        var combinedFields = new List<Field>();

                        // Copy fields from the first table's record
                        foreach (var field in record.Fields)
                        {
                            combinedFields.Add(new Field(new Column(field.Column.Name, field.Column.DataType), field.Value));
                        }

                        // Copy fields from the second table's record
                        foreach (var field in otherRecord.Fields)
                        {
                            combinedFields.Add(new Field(new Column(field.Column.Name, field.Column.DataType), field.Value));
                        }

                        // Add new combined record to the result table
                        resultTable.AddRecord(new Record(combinedFields));
                    }
                }
            }

            return resultTable;
        }
    }
}
