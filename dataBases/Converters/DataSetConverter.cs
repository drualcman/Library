using drualcman.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Converters
{
    public class DataSetConverter
    {
        public static string ToJson(DataSet ds)
            => ds.ToJson();
    }
}
