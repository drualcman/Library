using System;
using System.Data.SqlClient;

namespace drualcman
{
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
                throw;
            }
        }
    }
}
