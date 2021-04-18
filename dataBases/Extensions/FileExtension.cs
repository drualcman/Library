using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class FileExtension
    {
        #region Methods
        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static DataTable FromFile(this DataTable dt, string filePath, char separator)
        {
            StreamReader sr = new StreamReader(filePath);
            return dt.FromStream(sr.BaseStream, separator);
        }
        #endregion

        #region Async
        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static async Task<DataTable> FromFileAsync(this DataTable dt, string filePath, char separator)
        {
            StreamReader sr = new StreamReader(filePath);
            return await dt.FromStreamAsync(sr.BaseStream, separator);
        }
        #endregion
    }
}
