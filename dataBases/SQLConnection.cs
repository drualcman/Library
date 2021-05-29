using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        protected SqlConnection DbConnection;

        #region Open Connection
        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        protected void OpenConnection()
        {
            this.OpenConnection(this.rutaDDBB);
        }

        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        /// <param name="connectionString">Ruta alternativa de base de datos</param>
        protected async void OpenConnection(string connectionString)
        {
            await OpenConnectionAsync(connectionString);
        }

        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        protected async Task OpenConnectionAsync()
        {
            await this.OpenConnectionAsync(this.rutaDDBB);
        }

        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        /// <param name="connectionString">Ruta alternativa de base de datos</param>
        protected async Task OpenConnectionAsync(string connectionString)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("cnnDDBB(RutaDDBB)", connectionString, "");
            try
            {
                if (this.DbConnection is null)
                {
                    this.DbConnection = new SqlConnection(connectionString);
                    await this.DbConnection.OpenAsync();
                }
                else
                {
                    if (this.DbConnection.State != System.Data.ConnectionState.Open)
                        await this.DbConnection.OpenAsync();
                }
                if (this.LogError) log.end(this.DbConnection, this.rutaDDBB);
                log.Dispose();
            }
            catch (Exception ex)
            {
                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                log.Dispose();
                throw;
            }
        }
        #endregion

        #region Get Connection
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
        /// <param name="connectionString">Ruta alternativa de base de datos</param>
        /// <returns>
        /// Devuelve un SqlConnection
        /// Si hay error devuelve el mensaje del error
        /// </returns>
        public SqlConnection cnnDDBB(string connectionString)
        {
            this.OpenConnection(rutaDDBB);
            return this.DbConnection;
        }
    }
    #endregion
}

