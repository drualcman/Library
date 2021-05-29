using drualcman.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data
{
    class Columns
    {
        public PropertyInfo Column { get; set; }
        public DatabaseAttribute Options { get; set; }
    }
}
