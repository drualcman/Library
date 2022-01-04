using drualcman.Attributes;
using System.Reflection;

namespace drualcman.Data
{
    public class Columns
    {
        public string TableShortName { get; set; }
        public int TableIndex { get; set; }
        public string ColumnName { get; set; }
        public string PropertyType { get; set; }
        public PropertyInfo Column { get; set; }
        public DatabaseAttribute Options { get; set; }
    }
}
