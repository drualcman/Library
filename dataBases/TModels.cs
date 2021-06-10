using drualcman.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public TModel Get<TModel>(string query = "") where TModel : new()
        {
            List<TModel> list = List<TModel>(query);
            if (list.Any()) return list[0];
            else return new TModel();
        }
        #endregion

        #region async
        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string query = "") where TModel : new()
        {
            List<TModel> list = await ListAsync<TModel>(query);
            if (list.Any()) return list[0];
            else return new TModel();
        }
        #endregion
    }
}
