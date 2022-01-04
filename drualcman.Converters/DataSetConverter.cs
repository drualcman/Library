using drualcman.Converters.Extensions;
using System.Data;

namespace drualcman.Converters
{
    public class DataSetConverter
    {
        public static string ToJson(DataSet ds)
            => ds.ToJson();
    }
}
