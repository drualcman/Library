using drualcman.Enums;
using System.Reflection;

namespace drualcman.Data
{
    /// <summary>
    /// Setup inner model relationship
    /// </summary>
    public record TableName(string Name, string ShortName,
        string ShortReference, InnerDirection Inner, string Column,
        string InnerIndex, string ClassName, PropertyInfo Instance = null);
}
