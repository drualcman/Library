using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet DataSet<TModel>() => DataSet<TModel>(30);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timehout"></param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet DataSet<TModel>(int timehout) => DataSet(SetQuery<TModel>(), timehout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet ConsultarConDataSet(string querySQL) => ConsultarConDataSet(querySQL, 30);

        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet DataSet(string query, int timeout) => ConsultarConDataSet(query, timeout);

        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet DataSet(string query) => ConsultarConDataSet(query, 30);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet ConsultarConDataSet(string querySQL, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConDataSet", querySQL, "");
            // Que no sea una cadena vacía
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("Query can't be null");
            }
            else
            {
                try
                {
                    // Comprobar que están indicando valores correctos (o casi)
                    CheckSqlInjection(querySQL, log);

                    using SqlConnection con = new SqlConnection(this.rutaDDBB);
                    using SqlDataAdapter da = new SqlDataAdapter(querySQL, con);
                    da.SelectCommand.CommandTimeout = timeout;
                    using DataSet ds = new DataSet();                    
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();
                        throw;
                    }
                    finally
                    {
                        con.Close();
                        if (this.LogError) log.end(ds, this.rutaDDBB);
                        log.Dispose();
                    }

                    return ds;
                }
                catch (Exception ex)
                {
                    log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                    log.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL)
        {
            return spConDataset(querySQL, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, int timeout)
        {
            if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                querySQL = "EXEC " + querySQL;

            return ConsultarConDataSet(querySQL, timeout);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string param)
        {
            return spConDataset(querySQL, param, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string param, int timeout)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return spConDataset(querySQL + " " + param, timeout);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string[] param)
        {
            return spConDataset(querySQL, param, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string[] param, int timeout)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return spConDataset(querySQL + " " + doParam, timeout);
        }
        #endregion region

        #region tasks
        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<DataSet> DataSetAsync<TModel>() =>
            await DataSetAsync<TModel>(30);
        

        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<DataSet> DataSetAsync<TModel>(int timehout) =>
            await DataSetAsync(SetQuery<TModel>());

        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<DataSet> DataSetAsync(string query) =>
            await DataSetAsync(query, 30);

        /// <summary>
        /// Return DataSet from a direct query
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public Task<DataSet> DataSetAsync(string query, int timeout) =>
            Task.FromResult(ConsultarConDataSet(query, timeout));
        #endregion

    }
}
