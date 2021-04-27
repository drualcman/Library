using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Attributes
{
    public class DatabaseAttribute : Attribute
    {
        public string TableName { get; set; }
        public bool Ignore { get; set; }
        public bool Required { get; set; }
        public bool IndexKey { get; set; }
    }
}
