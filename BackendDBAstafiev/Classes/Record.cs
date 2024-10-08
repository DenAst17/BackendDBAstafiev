using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDBAstafiev.Classes
{

    public class Record
    {
        public List<Field> Fields { get; set; }

        public Record()
        {
            Fields = new List<Field>();
        }

        public Record(List<Field> fields)
        {
            Fields = fields;
        }

        public void AddField(Field field)
        {
            Fields.Add(field);
        }

        public void Save(StreamWriter writer)
        {
            writer.Write("Record: ");
            foreach (var field in Fields)
            {
                writer.Write($"{field.Value},");
            }
            writer.WriteLine();
        }

        public static Record Load(string lineToLoad, List<Column> columns)
        {
            Record record = new Record();
            string line = lineToLoad.Replace("Record: ", "");
            if (line != null)
            {
                string[] values = line.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    record.AddField(new Field(columns[i], values[i]));
                }
            }
            return record;
        }
    }

}
