using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestObzore
{
    public class ColumnData
    {
        public string Name { get; set; }
        public List<string> UniqueValues { get; set; } = new List<string>();
    }

}
