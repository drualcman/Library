using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drualcman.Data.Extensions;

namespace drualcman.Data.Converters
{
    public class DataTableConverter
    {
        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static DataTable FromStream(Stream data, char separator)
        {
            DataTable dt = new DataTable();            
            return dt.FromStream(data, separator);
        }
    }
}
