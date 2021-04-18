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
        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<TModel> Data<TModel>(string query) where TModel: new ()
        {   
            DataTable dt = DataTable(query);                //execute the query
            return dt.ToList<TModel>();          
        }
        #endregion

        #region async
        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<TModel>> DataAsync<TModel>(string query) where TModel : new()
        {
            DataTable dt = await DataTableAsync(query);                //execute the query
            return await dt.ToListAsync<TModel>();          
        }
        #endregion
    }
}
