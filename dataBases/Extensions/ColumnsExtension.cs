using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class ColumnsExtension
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

            foreach (DataColumn item in dr.ItemArray)
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
        public static List<string> ColumnNamesToList(this DataTable dt)
        {
            List<string> names = new List<string>();

            foreach (DataColumn item in dt.Columns)
            {
                names.Add(item.ColumnName);
            }

            return names;
        }

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string[] ColumnNamesToArray(this DataTable dt)
        {
            List<string> names = new List<string>();

            foreach (DataColumn item in dt.Columns)
            {
                names.Add(item.ColumnName);
            }

            return names.ToArray();
        }

        #endregion

        #region async

        /// <summary>
        /// Get all column names from the row send
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Task<string[]> ColumnNamesToArrayAsync(this DataRow dr) => Task.FromResult(dr.ColumnNamesToArray());

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<List<string>> ColumnNamesToListAsync(this DataTable dt) => Task.FromResult(dt.ColumnNamesToList());

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<string[]> ColumnNamesToArrayAsync(this DataTable dt) => Task.FromResult(dt.ColumnNamesToArray());

        #endregion
    }
}
