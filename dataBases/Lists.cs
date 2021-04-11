using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        public List<T> Data<T>(string query) where T: new ()
        {   
            DataTable dt = DataTable(query);                //execute the query
            return SetList<T>(dt);          //no results
        }
        #endregion

        #region tasks
        public async Task<List<T>> DataAsync<T>(string query) where T : new()
        {
            DataTable dt = await DataTableAsync(query);                //execute the query
            return SetList<T>(dt);          //no results
        }
        #endregion

        #region helpers
        public List<T> SetList<T>(DataTable dt) where T : new()
        {
            if (dt.Rows.Count > 0)
            {
                List<T> result = new List<T>();
                string[] columns = GetColumnNames(dt);
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (DataRow row in dt.Rows)
                {
                    T item = new T();

                    foreach (PropertyInfo pi in properties)
                    {
                        if (columns.Contains(pi.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            try
                            {
                                pi.SetValue(item, row[pi.Name], null);
                            }
                            catch
                            {

                            }
                        }
                    }

                    result.Add(item);
                }
                return result;
            }
            else return new List<T>();          //no results
        }
        #endregion
    }
}
