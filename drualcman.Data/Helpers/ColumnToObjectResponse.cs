using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Helpers
{
    public class ColumnToObjectResponse
    {
        public object InUse { get; set; }
        public string ActualTable { get; set; }
        public bool IsList { get; set; }
        public object PropertyListData { get; set; }
        public string PropertyListName { get; set; }
        public object PropertyListInstance { get; set; }

    }
}
