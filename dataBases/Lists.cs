using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using drualcman.Data.Extensions;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        public List<TModel> Data<TModel>(string query) where TModel: new ()
        {   
            DataTable dt = DataTable(query);                //execute the query
            return dt.ToList<TModel>();          
        }
        #endregion

        #region tasks
        public async Task<List<TModel>> DataAsync<TModel>(string query) where TModel : new()
        {
            DataTable dt = await DataTableAsync(query);                //execute the query
            return await dt.ToListAsync<TModel>();          
        }
        #endregion
    }
}
