using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct sql queries
        /// <summary>
        /// Ejecuta un comando SQL
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Retorna el valor obtenido del web service al ejecutar el comando
        /// </returns>
        public bool ComandoSQL(string sql)
        {
            return EjecutarSQL(sql);
        }

        /// <summary>
        /// Ejecuta un comando SQL
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Retorna el valor obtenido del web service al ejecutar el comando
        /// </returns>
        public bool EjecutarSQL(string sql)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("EjecutarSQL(sql)", sql, "");
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    //
                    // Comprobar que están indicando valores correctos (o casi)
                    //
                    // Que no sea una cadena vacía                    

                    if ((sql.ToUpper().IndexOf("UPDATE ") < 0))
                    {
                        if ((sql.ToUpper().IndexOf("INSERT ") < 0))
                        {
                            if ((sql.ToUpper().IndexOf("DELETE ") < 0))
                            {
                                if ((sql.ToUpper().IndexOf("EXEC ") < 0))
                                {
                                    if ((sql.ToUpper().IndexOf("DROP ") < 0))
                                    {
                                        if ((sql.ToUpper().IndexOf("ALTER ") < 0))
                                        {
                                            if ((sql.ToUpper().IndexOf("CREATE ") < 0))
                                            {
                                                string err = "La cadena debe ser: " + "\r\n" +
                                                    "UPDATE < tabla > SET < campo=valor >" + "\r\n" +
                                                    "INSERT INTO < tabla > VALUES < campo=valor >" + "\r\n" +
                                                    "DELETE < tabla > WHERE < condicion >" + "\r\n" +
                                                    "EXEC  < Storage Proccess > < Varaibles >" + "\r\n" +
                                                    "CREATE TABLE" + "\r\n" +
                                                    "DROP TABLE/PROCEDURE/FUNCTION < tabla >" + "\r\n" +
                                                    "ALTER TABLE < tabla > < definicion >" + "\r\n" +
                                                    "SQL: " + sql + "\n" + this.rutaDDBB;
                                                log.end(null, err);


                                                throw new ArgumentException(err);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else
                    {
                        bool retorno = ExecuteCommand(sql);
                        if (this.LogResults) log.end(retorno, this.rutaDDBB);

                        return retorno;
                    }

                }
                else
                    return false;
            }
        }

        public object Execute(string sql)
        {
            return Execute(sql, 30);
        }

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="sql">Query to execute</param>
        /// <param name="timeout">seconds to get a timeout</param>
        /// <returns></returns>
        public object Execute(string sql, int timeout)
        {
            object result;
            if (!string.IsNullOrEmpty(sql))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                result = Execute(cmd, timeout);
                cmd.Dispose();
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Execute a command. Only return false if exception.
        /// </summary>
        /// <param name="sql">Query to execute</param>
        /// <returns></returns>
        public bool ExecuteCommand(string sql)
        {
            return ExecuteCommand(sql, 30);
        }

        /// <summary>
        /// Execute a command. Only return false if exception.
        /// </summary>
        /// <param name="sql">command with the data</param>
        /// <param name="timeout">seconds to get a timeout</param>
        /// <returns></returns>
        public bool ExecuteCommand(string sql, int timeout)
        {
            bool result;
            if (!string.IsNullOrEmpty(sql))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                result = ExecuteCommand(cmd, timeout);
                cmd.Dispose();
            }
            else
            {
                result = false;
            }
            return result;
        }

        #region task
        public async Task<object> ExecuteAsync(string query, int timeout = 30)
        {
            object result;
            if (!string.IsNullOrEmpty(query))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                result = await ExecuteAsync(cmd, timeout);
                cmd.Dispose();
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Execute SQL query Asynchronous
        /// </summary>
        /// <param name="query"></param>
        /// <param name="timeout">default 30</param>
        /// <returns></returns>
        public async Task<bool> ExecuteCommandAsync(string query, int timeout = 30)
        {
            bool result;
            if (!string.IsNullOrEmpty(query))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                result = await ExecuteCommandAsync(cmd, timeout);
                cmd.Dispose();
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion
        #endregion

        #region direct SP queries
        /// <summary>
        /// Ejecutar un SP en el servidor devuelve XML con los datos
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string sql)
        {
            if ((sql.ToUpper().IndexOf("EXEC ") < 0))
                sql = "EXEC " + sql;

            return ConsultarConXML(sql);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string sql, string param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return EjecutarSP(sql + " " + param);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string sql, string[] param)
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

            return EjecutarSP(sql + " " + doParam);
        }

        #region tasks
        public Task<string> ExecuteSPAsync(string query) => Task.FromResult(EjecutarSP(query));
        public Task<string> ExecuteSPAsync(string query, string param) => Task.FromResult(EjecutarSP(query, param));
        public Task<string> ExecuteSPAsync(string query, string[] param) => Task.FromResult(EjecutarSP(query, param));
        #endregion
        #endregion
    }
}
