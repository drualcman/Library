using drualcman.Converters.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.FromStream")]
        public static DataTable FromStream(Stream data, char separator) =>
            drualcman.Converters.DataTableConverter.FromStream(data, separator);


        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.FromJson")]
        public static DataTable FromJson(string data) =>
            drualcman.Converters.DataTableConverter.FromJson(data);

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.ToList")]
        public static List<TModel> ToList<TModel>(DataTable dt) where TModel : new()
            => dt.ToList<TModel>();

        /// <summary>
        /// Convert DataTable to Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.ToJson")]
        public static string ToJson(DataTable dt)
            => dt.ToJson();
        #endregion

        #region async
        /// <summary>
        /// get aDataTable from Stream Data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.FromStreamAsync")]
        public static Task<DataTable> FromStreamAsync(Stream data, char separator) =>
            drualcman.Converters.DataTableConverter.FromStreamAsync(data, separator);

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.ToListAsync")]
        public static Task<List<TModel>> ToListAsync<TModel>(DataTable dt) where TModel : new()
            => dt.ToListAsync<TModel>();

        #endregion
    }
}
