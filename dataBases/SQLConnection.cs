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
        protected void OpenConnection(string connectionString)
        {
            //bool b = await OpenConnectionAsync();

            log.start("cnnDDBB(RutaDDBB)", connectionString, "");
            try
            {
                if (this.DbConnection is null)
                {
                    this.DbConnection = new SqlConnection(connectionString);
                    this.DbConnection.Open();
                }
                else
                {
                    if (this.DbConnection.State != System.Data.ConnectionState.Open)
                        this.DbConnection.Open();
                }
                if (this.LogResults) log.end(this.DbConnection, this.rutaDDBB);
            }
            catch (Exception ex)
            {
                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                throw;
            }
        }

        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        protected Task OpenConnectionAsync()
        {
            return this.OpenConnectionAsync(this.rutaDDBB);
        }

        /// <summary>
        /// Open a connection with a DB
        /// </summary>
        /// <param name="connectionString">Ruta alternativa de base de datos</param>
        protected Task OpenConnectionAsync(string connectionString)
        {
            this.OpenConnection(connectionString);
            return Task.CompletedTask;
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

