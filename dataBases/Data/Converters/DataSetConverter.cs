﻿using drualcman.Converters.Extensions;
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
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataSetConverter.ToJson")]
        public static string ToJson(DataSet ds)
            => ds.ToJson();
    }
}
