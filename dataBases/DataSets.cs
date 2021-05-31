using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet ConsultarConDataSet(string sql) => ConsultarConDataSet(sql, 30);

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
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet ConsultarConDataSet(string sql, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConDataSet", sql, "");
            // Que no sea una cadena vacía
            if (string.IsNullOrWhiteSpace(sql))
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
                    CheckSqlInjection(sql, log);

                    string input = sql;
                    string pattern = "\\[t[0-9].";
                    string replacement = "[";
                    sql = Regex.Replace(input, pattern, replacement);

                    this.OpenConnection();
                    using SqlDataAdapter da = new SqlDataAdapter(sql, this.DbConnection);
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
                        if (this.LogError) log.end(ds, this.rutaDDBB);
                        log.Dispose();
                    }

                    return ds;
                }
                catch (Exception ex)
                {
                    log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);
                    log.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql)
        {
            return spConDataset(sql, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql, int timeout)
        {
            if ((sql.ToUpper().IndexOf("EXEC ") < 0))
                sql = "EXEC " + sql;

            return ConsultarConDataSet(sql, timeout);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql, string param)
        {
            return spConDataset(sql, param, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql, string param, int timeout)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return spConDataset(sql + " " + param, timeout);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql, string[] param)
        {
            return spConDataset(sql, param, 30);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string sql, string[] param, int timeout)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return spConDataset(sql + " " + doParam, timeout);
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
