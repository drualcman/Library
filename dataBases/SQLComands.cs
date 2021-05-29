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
                    if (cmd.Connection == null) cmd.Connection = this.cnnDDBB();
                    cmd.Connection.Open();
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
                    _ = cmd.Connection.DisposeAsync();
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

            if (cmd != null)
            {
                log.start("Execute(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    if (cmd.Connection == null) cmd.Connection = this.cnnDDBB();
                    cmd.Connection.Open();
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
                    _ = cmd.Connection.DisposeAsync();
                    _ = cmd.DisposeAsync();
                }
            }
            else
            {
                log.start("Execute(cmd)", "", this.rutaDDBB);
                result = null;
                log.end(result.ToString(), "CMD is null");
            }

            return result;
        }
        #endregion
    }

}