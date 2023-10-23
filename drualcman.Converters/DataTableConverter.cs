using drualcman.Converters.Extensions;
using System.Data;

namespace drualcman.Converters
{
    public class DataTableConverter
    {
        #region methods
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


        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        public static DataTable FromJson(string data)
        {
            DataTable dt = new DataTable();
            return dt.FromJson(data);
        }

        /// <summary>
        /// Convert DataTable to Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
            => dt.ToJson();
        #endregion

        #region async
        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static Task<DataTable> FromStreamAsync(Stream data, char separator)
        {
            DataTable dt = new DataTable();
            return dt.FromStreamAsync(data, separator);
        }
        #endregion
    }
}
