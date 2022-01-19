using System.Data;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// </returns>
        public DataView DataView<TModel>() =>
            DataView<TModel>(30);

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataView DataView<TModel>(int timeout) =>
            ConsultarConDataView(SetQuery<TModel>(), timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataView ConsultarConDataView(string sql)
        {
            return ConsultarConDataView(sql, 30);
        }

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataView DataView(string sql) =>
            ConsultarConDataView(sql, 30);

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataView DataView(string sql, int timeout) =>
            ConsultarConDataView(sql, timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataView ConsultarConDataView(string sql, int timeout)
        {

            log.start("ConsultarConDataView", sql, "");
            return ConsultarConDataTable(sql, timeout).DefaultView;
        }
        #endregion

        #region tasks

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// </returns>
        public async Task<DataView> DataViewAsync<TModel>() =>
            await DataViewAsync<TModel>(30);

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public async Task<DataView> DataViewAsync<TModel>(int timeout) =>
            await DataViewAsync(SetQuery<TModel>(), 30);

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public async Task<DataView> DataViewAsync(string query) => await DataViewAsync(query, 30);

        /// <summary>
        /// Return DataView
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public async Task<DataView> DataViewAsync(string query, int timeout)
        {
            DataSet ds = await DataSetAsync(query, timeout);
            DataView dv = ds.Tables[0].DefaultView;
            return dv;
        }
        #endregion
    }
}
