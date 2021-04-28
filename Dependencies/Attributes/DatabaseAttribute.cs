using System;

namespace drualcman.Attributes
{
    public class DatabaseAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Ignore { get; set; }
        public bool IndexKey { get; set; }
        public string IndexedName { get; set; }
    }
}
