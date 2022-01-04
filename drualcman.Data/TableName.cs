using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data
{
    /// <summary>
    /// Setup inner model relationship
    /// </summary>
    public record TableName(string Name, string ShortName,
        string ShortReference, InnerDirection Inner, string Column,
        string InnerIndex, string ClassName, PropertyInfo Instance = null);
}
