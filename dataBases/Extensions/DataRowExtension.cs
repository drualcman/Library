using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class DataRowExtension
    {
        #region methods

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> ColumnNamesToList(this DataRow dr)
        {
            List<string> names = new List<string>();

            foreach (DataColumn item in dr.ItemArray)
            {
                names.Add(item.ColumnName);
            }

            return names;
        }
        #endregion

        #region Async
        public static Task<List<string>> ColumnNamesToListAsync(this DataRow dr) => Task.FromResult(dr.ColumnNamesToList());
        #endregion


    }
}
