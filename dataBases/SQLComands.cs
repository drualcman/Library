using System.Data.SqlClient;

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
        /// <param name="timeout">max seconds to execute the command</param>
        /// <returns></returns>
        public bool ExecuteCommand(SqlCommand cmd, int timeout = 30)
        {

            bool result;

            if(cmd != null)
            {
                log.start("ExecuteCommand(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    using SqlConnection cn = new SqlConnection(this.connectionString);
                    cmd.Connection = cn;
                    cmd.CommandTimeout = timeout;
                    cmd.Connection.Open();
                    cmd.CommandTimeout = timeout;
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    result = true;
                    if(this.LogResults) log.end(result.ToString());
                }
                catch(Exception ex)
                {
                    result = false;
                    log.end(result, ex);
                }
                finally
                {
                    cmd.Dispose();
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

        /// <summary>
        /// Execute a command and return the value
        /// </summary>
        /// <param name="cmd">command with the data</param>
        /// <param name="timeout">max seconds to execute the command</param>
        /// <returns></returns>
        public object Execute(SqlCommand cmd, int timeout = 30)
        {

            object result;

            log.start("Execute(cmd)", cmd.CommandText, this.rutaDDBB);
            try
            {
                using SqlConnection cn = new SqlConnection(this.connectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = timeout;
                cmd.Connection.Open();
                cmd.CommandTimeout = timeout;
                result = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if(this.LogResults) log.end(result?.ToString());
            }
            catch(Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {

                cmd.Dispose();
            }
            return result;
        }


        public SqlDataReader Reader(string sql, int timeout = 30)
        {

            SqlDataReader result;
            log.start("Reader(sql)", sql, this.rutaDDBB);
            #region query
            // If a query is empty create the query from the Model
            // check the sql if have some injection throw a exception
            CheckSqlInjection(sql, log);
            #endregion

            try
            {
                using SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                result = Reader(cmd, timeout);
            }
            catch(Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            return result;
        }

        public SqlDataReader Reader(SqlCommand cmd, int timeout = 30)
        {

            SqlDataReader result;
            log.start("Reader(cmd)", cmd.CommandText, this.rutaDDBB);
            try
            {
                using SqlConnection cn = new SqlConnection(this.connectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = timeout;
                cmd.Connection.Open();
                cmd.CommandTimeout = timeout;
                result = cmd.ExecuteReader();
                cmd.Connection.Close();
                if(this.LogResults) log.end(result);
            }
            catch(Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return result;
        }
        #endregion

        #region task
        public async Task<bool> ExecuteCommandAsync(SqlCommand cmd, int timeout = 30)
        {

            bool result;

            if(cmd != null)
            {
                log.start("ExecuteCommand(cmd)", cmd.CommandText, this.rutaDDBB);
                try
                {
                    using SqlConnection cn = new SqlConnection(this.connectionString);
                    cmd.Connection = cn;
                    cmd.CommandTimeout = timeout;
                    await cmd.Connection.OpenAsync();
                    cmd.CommandTimeout = timeout;
                    await cmd.ExecuteNonQueryAsync();
                    result = true;
                    await cmd.Connection.CloseAsync();
                    if(this.LogResults) log.end(result.ToString());
                }
                catch(Exception ex)
                {
                    result = false;
                    log.end(result, ex);
                }
                finally
                {
                    await cmd.DisposeAsync();
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

        public async Task<object> ExecuteAsync(SqlCommand cmd, int timeout = 30)
        {

            object result;

            log.start("Execute(cmd)", cmd.CommandText, this.rutaDDBB);
            try
            {

                using SqlConnection cn = new SqlConnection(this.connectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = timeout;
                await cmd.Connection.OpenAsync();
                cmd.CommandTimeout = timeout;
                result = await cmd.ExecuteScalarAsync();
                await cmd.Connection.CloseAsync();
                if(this.LogResults) log.end(result?.ToString());
            }
            catch(Exception ex)
            {
                result = null;
                log.end(result, ex);
            }
            finally
            {

                await cmd.DisposeAsync();
            }


            return result;
        }

        public async Task<SqlDataReader> ReaderAsync(string sql, int timeout = 30) =>
            await Task.FromResult(Reader(sql, timeout));

        public async Task<SqlDataReader> ReaderAsync(SqlCommand cmd, int timeout = 30) =>
            await Task.FromResult(Reader(cmd, timeout));
        #endregion
    }

}