using System.Data;


namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases : IDisposable
    {
        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> AsEnumerable(DataTable table)
        {
            for(int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }

        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IQueryable<DataRow> AsQuerable(DataTable table)
        {
            return AsEnumerable(table).AsQueryable();
        }

        /*
         * https://www.dotnetperls.com/datatable-compare-rows
         * https://www.codeproject.com/Questions/1224003/Compare-two-datatables-in-Csharp
         * https://social.msdn.microsoft.com/Forums/es-ES/15454cdb-f869-4671-aac5-bc37f40f949c/como-comparar-dos-datatables?forum=vcses
         * https://docs.microsoft.com/es-es/dotnet/api/system.data.datatable.select?redirectedfrom=MSDN&view=netframework-4.7.2#overloads
         */
        /// <summary>
        /// Comparar tablas que tendan un ID unico. La estructura de las tablas debe de ser exactamente la misma.
        /// </summary>
        /// <param name="dtOrigin">Tabla origen</param>
        /// <param name="dtDestination">Tabla a comparar</param>
        /// <param name="columnID">Columna con el nombre del identificador unico.</param>
        /// <returns></returns>
        public static DataTable TableDifferences(DataTable dtOrigin, DataTable dtDestination, string columnID)
        {
            DataTable dtResult;
            dtResult = dtOrigin.Clone();
            foreach(DataRow origen in dtOrigin.Rows)
            {
                string expression = columnID + " = '" + origen[columnID].ToString() + "'";
                DataRow[] destino = dtDestination.Select(expression);
                if(destino.Count() == 0) dtResult.Rows.Add(origen.ItemArray);
            }
            return dtResult;
        }

        /// <summary>
        /// Comparar tablas que tendan un ID unico. La estructura de las tablas debe de ser exactamente la misma.
        /// </summary>
        /// <param name="dtOrigin">Tabla origen</param>
        /// <param name="dtDestination">Tabla a comparar</param>
        /// <param name="columnID">Columna con el nombre del identificador unico.</param>
        /// <returns></returns>
        public DataTable tableDifferences(DataTable dtOrigin, DataTable dtDestination, string columnID)
        {
            return dataBases.TableDifferences(dtOrigin, dtDestination, columnID);
        }

        /// <summary>
        /// Devolver el nombre de la base de datos
        /// </summary>
        /// <returns></returns>
        public string GetCatalog()
        {
            string[] ruta = this.rutaDDBB.Split(';');
            string catalog = string.Empty;
            foreach(string Categoria in ruta)
            {
                string[] desglose = Categoria.Split('=');
                if(desglose[0].ToLower().IndexOf("catalog") >= 0) catalog = desglose[1];
            }
            return catalog;
        }

        /// <summary>
        /// Devolver el nombre del servidor
        /// </summary>
        /// <returns></returns>
        public string GetServer()
        {
            string[] ruta = this.rutaDDBB.Split(';');
            string Source = string.Empty;
            foreach(string Categoria in ruta)
            {
                string[] desglose = Categoria.Split('=');
                if(desglose[0].ToLower().IndexOf("source") >= 0) Source = desglose[1];
            }
            return Source;
        }

    }
}