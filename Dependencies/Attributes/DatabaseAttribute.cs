using drualcman.Enums;

namespace drualcman.Attributes
{
    public class DatabaseAttribute : Attribute
    {
        /// <summary>
        /// Column name or table name if it's different from the property name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Don't include in the select actions
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// Indicate this is a index required column to inject from the default where actions
        /// </summary>
        public bool IndexKey { get; set; }
        /// <summary>
        /// Column name about the index key if it's different from the property name
        /// </summary>
        public string IndexedName { get; set; }
        /// <summary>
        /// Order rows with join order
        /// </summary>
        public int InnerOrder { get; set; }
        /// <summary>
        /// Join direction
        /// </summary>
        public InnerDirection Inner { get; set; } = InnerDirection.NONE;
        /// <summary>
        /// Column name to join in a 2nd table
        /// </summary>
        public string InnerColumn { get; set; }
        /// <summary>
        /// Column name to join in a 1st table
        /// </summary>
        public string InnerIndex { get; set; }
    }
}
