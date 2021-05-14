﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drualcman.Data.Extensions;

namespace drualcman.Data.Converters
{
    public class DataTableConverter
    {

        #region methods
        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static DataTable FromStream(Stream data, char separator)
        {
            DataTable dt = new DataTable();            
            return dt.FromStream(data, separator);
        }


        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        public static DataTable FromJson(string data)
        {
            DataTable dt = new DataTable();
            return dt.FromJson(data);
        }

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(DataTable dt) where TModel : new()
            => dt.ToList<TModel>();

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(DataTable dt, string[] columns) where TModel : new() 
            => dt.ToList<TModel>(columns);

        /// <summary>
        /// Convert DataTable to Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
            => dt.ToJson();
        #endregion

        #region async
        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static async Task<DataTable> FromStreamAsync(Stream data, char separator)
        {
            DataTable dt = new DataTable();
            return await dt.FromStreamAsync(data, separator);
        }

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<List<TModel>> ToListAsync<TModel>(DataTable dt) where TModel : new()
            => await dt.ToListAsync<TModel>();

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<List<TModel>> ToListAsync<TModel>(DataTable dt, string[] columns) where TModel : new()
            => await dt.ToListAsync<TModel>(columns);
        #endregion
    }
}
