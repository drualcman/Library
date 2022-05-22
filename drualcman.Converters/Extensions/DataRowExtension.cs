using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace drualcman.Converters.Extensions
{
    public static class DataRowExtension
    {
        #region methods
        /// <summary>
        /// Get all column names from the row send
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string[] ColumnNamesToArray(this DataRow dr)
        {
            List<string> names = new List<string>();

            foreach(DataColumn item in dr.Table.Columns)
            {
                names.Add(item.ColumnName);
            }

            return names.ToArray();
        }

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> ColumnNamesToList(this DataRow dr)
        {
            List<string> names = new List<string>();

            foreach(DataColumn item in dr.ItemArray)
            {
                names.Add(item.ColumnName);
            }

            return names;
        }
        #endregion

        #region Async
        /// <summary>
        /// Get all column names from the row send
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Task<string[]> ColumnNamesToArrayAsync(this DataRow dr) => Task.FromResult(dr.ColumnNamesToArray());

        public static Task<List<string>> ColumnNamesToListAsync(this DataRow dr) => Task.FromResult(dr.ColumnNamesToList());
        #endregion


    }
}
