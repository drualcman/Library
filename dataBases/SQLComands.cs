using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace drualcman
{
    /// <summary>
    /// Using SQLConnection to execute SQLCommands
    /// </summary>
    public partial class dataBases
    {
        #region methods
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
            return ExecuteCommandAsync(cmd, timeout).Result;
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
            return ExecuteAsync(cmd, timeout).Result;
        }

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="sql">Query to execute</param>
        /// <returns></returns>
        #endregion

        #region task
        public async Task<bool> ExecuteCommandAsync(SqlCommand cmd) => await ExecuteCommandAsync(cmd, 30);
        public async Task<bool> ExecuteCommandAsync(SqlCommand cmd, int timeout) 
        {
            defLog log = new defLog(this.FolderLog);
            bool result;

            if (cmd != null)
            {
                log.start("ExecuteCommand(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    if (cmd.Connection is null)
                    {
                        await this .OpenConnectionAsync();
                        cmd.Connection = this.DbConnection;
                    }
                    else
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Open) cmd.Connection.Open();
                    }
                    cmd.CommandTimeout = timeout;
                    await cmd.ExecuteNonQueryAsync();
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
                    _ = cmd.DisposeAsync();
                }
            }
            else
            {
                log.start("ExecuteCommand(cmd)", "", this.rutaDDBB);
                result = false;
                log.end(result.ToString(), "CMD is null");
            }

            return result;
        }
        public async Task<object> ExecuteAsync(SqlCommand cmd) => await ExecuteAsync(cmd, 30);
        public async Task<object> ExecuteAsync(SqlCommand cmd, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            object result;

            log.start("Execute(cmd)", cmd.CommandText, this.rutaDDBB);
            try
            {
                if (cmd.Connection == null)
                {
                    await this.OpenConnectionAsync();
                    cmd.Connection = this.DbConnection;
                }
                else
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                }
                cmd.CommandTimeout = timeout;
                result = await cmd.ExecuteScalarAsync();
                if (this.LogError) log.end(result.ToString());
            }
            catch (Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {

                _ = cmd.DisposeAsync();
            }


            return result;
        }

        public async Task<SqlDataReader> ReaderAsync(string sql) => await ReaderAsync(sql, 30);
        public async Task<SqlDataReader> ReaderAsync(string sql, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            SqlDataReader result;
            log.start("ReaderAsync(sql)", sql, this.rutaDDBB);
            #region query
            // If a query is empty create the query from the Model
            // check the sql if have some injection throw a exception
            CheckSqlInjection(sql, log);
            #endregion

            try
            {
                await this .OpenConnectionAsync();
                using SqlCommand cmd = this.DbConnection.CreateCommand();
                cmd.CommandText = sql;
                result = await ReaderAsync(cmd, timeout);
            }
            catch (Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {
                log.Dispose();
            }
            return result;
        }

        public async Task<SqlDataReader> ReaderAsync(SqlCommand cmd) => await ReaderAsync(cmd, 30);
        public async Task<SqlDataReader> ReaderAsync(SqlCommand cmd, int timeout)
        {
            defLog log = new defLog(this.FolderLog);
            SqlDataReader result;
            log.start("ReaderAsync(cmd)", cmd.CommandText, this.rutaDDBB);
            try
            {
                if (cmd.Connection == null)
                {
                    await this.OpenConnectionAsync();
                    cmd.Connection = this.DbConnection;
                }
                else
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                }
                cmd.CommandTimeout = timeout;
                result = await cmd.ExecuteReaderAsync();
                if (this.LogError) log.end(result.ToString());
            }
            catch (Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {
                _ = cmd.DisposeAsync();
            }
            return result;
        }
        #endregion
    }

}