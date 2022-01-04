using System;
using System.Data;
using System.Threading.Tasks;

namespace drualcman.Data.Converters
{
    public class CSVConverter
    {
        #region methods
        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataView dv)
            => GetCSV(dv, ",");

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataView dv, string separator)
            => GetCSV(dv.ToTable(), separator);

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataTable dt)
            => GetCSV(dt, ",");

        /// <summary>
        /// get a CSV file
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="separator">Separator to use</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataTable dt, string separator) =>
            drualcman.Converters.CSVConverter.GetCSV(dt, separator);

        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.CSVToDataTable")]
        public static DataTable CSVToDataTable(string filePath, char separator) =>
            drualcman.Converters.CSVConverter.CSVToDataTable(filePath, separator);
        #endregion

        #region Async

        /// <summary>
        /// Convert a csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.CSVConverter.CSVToDataTable")]
        public static Task<DataTable> CSVToDataTableAsync(string filePath, char separator) =>
            drualcman.Converters.CSVConverter.CSVToDataTableAsync(filePath, separator);
        #endregion

    }
}
