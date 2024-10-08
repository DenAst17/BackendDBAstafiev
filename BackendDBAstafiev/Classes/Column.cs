using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDBAstafiev.Classes
{
    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public Column(string name, string dataType)
        {
            Name = name;
            DataType = dataType;
        }
    }

}
