﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class DataSetExtension
    {
        #region Methods
        #region TO
        /// <summary>
        /// Convertir uyn DataSet en un objero JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ToJson(this DataSet ds)
        {
            //https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp                
            StringBuilder jsonString = new StringBuilder();
            int c = ds.Tables[0].Rows.Count;
            if (c > 0)
            {
                jsonString.Append("[");
                for (int t = 0; t < c; t++)
                {
                    jsonString.Append("{\"");
                    jsonString.Append(ds.Tables[t].TableName);
                    jsonString.Append("\":");
                    jsonString.Append(ds.Tables[t].ToJson());
                    jsonString.Append(t == c - 1 ? "}" : "},");
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            return string.Empty;
        }
        #endregion
        #endregion

        #region Async
        #region TO
        /// <summary>
        /// Convertir uyn DataSet en un objero JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Task<string> ToJsonAsync(this DataSet ds)
            => Task.FromResult(ToJson(ds));
        #endregion
        #endregion

    }
}
