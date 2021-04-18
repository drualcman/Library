using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class DataTableExtension
    {
        public static List<TModel> ToList<TModel>(this DataTable dt, string[] columns) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                List<TModel> result = new List<TModel>();
                PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (DataRow row in dt.Rows)
                {
                    TModel item = new TModel();

                    string[] rowCols = row.ColumnNamesToArray();
                    bool hasData = false;
                    foreach (PropertyInfo pi in properties)
                    {
                        if (columns.Contains(pi.Name, StringComparer.OrdinalIgnoreCase) && 
                            rowCols.Contains(pi.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            try
                            {
                                pi.SetValue(item, row[pi.Name], null);
                                hasData = true;
                            }
                            catch
                            {

                            }
                        }
                    }
                    if (hasData) result.Add(item);      //only add the item if have some to add
                }
                return result;
            }
            else return new List<TModel>();          //no results
        }

        public static List<TModel> ToList<TModel>(this DataTable dt) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                string[] columns = dt.ColumnNamesToArray();                
                return  dt.ToList<TModel>(columns);
            }
            else return new List<TModel>();          //no results
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

        #region async

        public static async Task<List<TModel>> ToListAsync<TModel>(this DataTable dt) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                string[] columns = await dt.ColumnNamesToArrayAsync();
                return dt.ToList<TModel>(columns);
            }
            else return new List<TModel>();          //no results
        }


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
