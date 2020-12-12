using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace drualcman
{
    /// <summary>
    /// Using SQLConnection to execute SQLCommands
    /// </summary>
    public partial class dataBases
    {
      
        /// <summary>
        /// Conector de la base de datos
        /// </summary>
        /// <returns>
        /// Devuelve un SqlConnection
        /// Si hay error devuelve el mensaje del error
        /// </returns>
        public SqlConnection cnnDDBB()
        {
            return this.cnnDDBB(this.rutaDDBB);
        }

        /// <summary>
        /// Conector de la base de datos
        /// </summary>
        /// <param name="RutaDDBB">Ruta alternativa de base de datos</param>
        /// <returns>
        /// Devuelve un SqlConnection
        /// Si hay error devuelve el mensaje del error
        /// </returns>
        public SqlConnection cnnDDBB(string RutaDDBB)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("cnnDDBB(RutaDDBB)", RutaDDBB, "");
            try
            {
                SqlConnection cnn = new SqlConnection(RutaDDBB);
                if (this.LogError) log.end(cnn, this.rutaDDBB);
                log.Dispose();

                return cnn;
            }
            catch (Exception ex)
            {
                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                log.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// Execute a command. Only return false if exception.
        /// </summary>
        /// <param name="cmd">command with the data</param>
        /// <returns></returns>
        public bool ExecuteCommand(SqlCommand cmd)
        {
            return ExecuteCommand(cmd, 30);
        }

        /// <summary>
        /// Execute a command. Only return false if exception.
        /// </summary>
        /// <param name="cmd">command with the data</param>
        /// <param name="timeout">max seconds to execute the command</param>
        /// <returns></returns>
        public bool ExecuteCommand(SqlCommand cmd, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            bool result;

            if (cmd != null)
            {
                log.start("ExecuteCommand(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    if (cmd.Connection == null) cmd.Connection = this.cnnDDBB();
                    cmd.Connection.Open();
                    cmd.CommandTimeout = timeout;
                    cmd.ExecuteNonQuery();
                    result = true;
                    if (this.LogError) log.end(result.ToString());
                }
                catch (Exception ex)
                {
                    result = false;
                    log.end(result, ex);
                }
                finally
                {
                    cmd.Connection.Close();
                }
                cmd.Dispose();
            }
            else
            {
                log.start("ExecuteCommand(cmd)", "", this.rutaDDBB);
                result = false;
                log.end(result.ToString(), "CMD is null");
            }

            return result;
        }

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="cmd">command with the data</param>
        /// <returns></returns>
        public object Execute(SqlCommand cmd)
        {
            return Execute(cmd, 30);
        }

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="cmd">command with the data</param>
        /// <param name="timeout">max seconds to execute the command</param>
        /// <returns></returns>
        public object Execute(SqlCommand cmd, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            object result;

            if (cmd != null)
            {
                log.start("Execute(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    if (cmd.Connection == null) cmd.Connection = this.cnnDDBB();
                    cmd.Connection.Open();
                    cmd.CommandTimeout = timeout;
                    result = cmd.ExecuteScalar();
                    if (this.LogError) log.end(result.ToString());
                }
                catch (Exception ex)
                {
                    result = null;
                    log.end(result, ex);
                }
                finally
                {
                    cmd.Connection.Close();
                }
                cmd.Dispose();
            }
            else
            {
                log.start("Execute(cmd)", "", this.rutaDDBB);
                result = null;
                log.end(result.ToString(), "CMD is null");
            }

            return result;
        }

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="sql">Query to execute</param>
        /// <returns></returns>
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
                result = Execute(cmd);
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

    }

}