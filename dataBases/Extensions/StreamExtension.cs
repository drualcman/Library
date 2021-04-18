using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class StreamExtension
    {
        #region Methods
        /// <summary>
        /// Parse DataTable from Stream Data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static DataTable FromStream(this DataTable dt, Stream data, char separator)
        {
            StreamReader sr = new StreamReader(data);
            string[] headers = sr.ReadLine().Split(separator);
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(separator);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region Async
        /// <summary>
        /// Parse DataTable from Stream Data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static async Task<DataTable> FromStreamAsync(this DataTable dt, Stream data, char separator)
        {
            StreamReader sr = new StreamReader(data);
            string head = await sr.ReadLineAsync();
            string[] headers = head.Split(separator);
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string rowHead = await sr.ReadLineAsync();
                string[] rows = rowHead.Split(separator);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
