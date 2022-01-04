using drualcman.Converters.Extensions;
using System.Data;
using System.Threading.Tasks;

namespace drualcman.Converters
{
    public class CSVConverter
    {
        #region methods
        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static string GetCSV(DataView dv)
            => GetCSV(dv, ",");

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static string GetCSV(DataView dv, string separator)
            => GetCSV(dv.ToTable(), separator);

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataTable dt)
            => GetCSV(dt, ",");

        /// <summary>
        /// get a CSV file
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="separator">Separator to use</param>
        /// <returns></returns>
        public static string GetCSV(DataTable dt, string separator)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + separator;
            }
            csv = csv.Remove(csv.Length - 1, 1);        //remove last character because is the separator    
            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(separator, "") + separator;
                }
                csv = csv.Remove(csv.Length - 1, 1);        //remove last character because is the separator                
                csv += "\r\n";                              //Add new line.
            }

            return csv;
        }

        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static DataTable CSVToDataTable(string filePath, char separator)
        {
            DataTable dt = new DataTable();
            return dt.FromFile(filePath, separator);
        }
        #endregion

        #region Async

        /// <summary>
        /// Convert a csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static async Task<DataTable> CSVToDataTableAsync(string filePath, char separator)
        {
            DataTable dt = new DataTable();
            return await dt.FromFileAsync(filePath, separator);

        }
        #endregion

    }
}
