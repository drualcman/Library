using System;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace drualcman.Converters.Extensions
{
    public static class ObjectConverter
    {
        #region Methods
        #region TO
        public static string ToJson(this object o)
        {
            string data;
            try
            {
                if(o != null)
                {
                    if(o.GetType() == typeof(DataSet))
                    {
                        data = DataSetConverter.ToJson((DataSet)o);
                    }
                    else if(o.GetType() == typeof(DataTable))
                    {
                        data = DataTableConverter.ToJson((DataTable)o);
                    } 
                    else if(o.GetType() == typeof(DataView))
                    {
                        DataView dv = (DataView)o;
                        DataTable dt = dv.ToTable();
                        data = dt.ToJson();
                    }
                    else data = JsonSerializer.Serialize(o);
                }
                else data = "{\"Object\":\"NULL\"}";
            }
            catch(Exception ex)
            {
                data = $"{{\"Exception\":\"{ex.Message}\"}}";
            }
            return data;
        }
        #endregion
        #endregion

        #region async
        #region TO
        public static Task<string> ToJsonAsync(this object o)
            => Task.FromResult(o.ToJson());
        #endregion
        #endregion
    }
}
