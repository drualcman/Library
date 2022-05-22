using System;
using System.Data;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable DataTable<TModel>() => DataTable<TModel>(30);

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable DataTable<TModel>(int timeout) => ConsultarConDataTable(SetQuery<TModel>(), timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable ConsultarConDataTable(string sql) => ConsultarConDataTable(sql, 30);


        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable DataTable(string sql) => ConsultarConDataTable(sql, 30);

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable DataTable(string sql, int timeout) => ConsultarConDataTable(sql, timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public DataTable ConsultarConDataTable(string sql, int timeout)
        {

            log.start("ConsultarConDataTable", sql, "");
            return ConsultarConDataSet(sql, timeout).Tables[0];
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql)
        {
            return spConDataTable(sql, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql, int timeout)
        {
            if((sql.ToUpper().IndexOf("EXEC ") < 0))
                sql = "EXEC " + sql;

            return ConsultarConDataTable(sql, timeout);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql, string param)
        {
            return spConDataTable(sql, param, 30);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql, string param, int timeout)
        {
            return spConDataTable(sql + " " + param, timeout);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql, string[] param)
        {
            return spConDataTable(sql, param, 30);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string sql, string[] param, int timeout)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if(string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach(string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return spConDataTable(sql + " " + doParam, timeout);
        }
        #endregion

        #region tasks

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// DataTable
        /// </returns>
        public async Task<DataTable> DataTableAsync<TModel>() => await DataTableAsync<TModel>(30);

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public async Task<DataTable> DataTableAsync<TModel>(int timeout) => await DataTableAsync(SetQuery<TModel>(), 30);

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public async Task<DataTable> DataTableAsync(string query) => await DataTableAsync(query, 30);

        /// <summary>
        /// Return DataTable from Query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// DataTable
        /// </returns>
        public async Task<DataTable> DataTableAsync(string query, int timeout)
        {
            DataSet ds = await DataSetAsync(query, timeout);
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }
        #endregion

    }
}
