using drualcman.Converters.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace drualcman
{
    /// <summary>
    /// Data conversion
    /// </summary>
    public partial class dataBases
    {

        #region CSV data
        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataView dv)
            => Converters.CSVConverter.GetCSV(dv);

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataView dt, string separator)
            => Converters.CSVConverter.GetCSV(dt, separator);

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataTable dt)
            => Converters.CSVConverter.GetCSV(dt, ",");

        /// <summary>
        /// get a CSV file
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="separator">Separator to use</param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.CSVConverter.GetCSV")]
        public static string GetCSV(DataTable dt, string separator)
            => Converters.CSVConverter.GetCSV(dt, separator);

        [Obsolete(message: "Use drualcman.Converters.DataTableConverter.FromStream")]
        public static DataTable CSVtoData(Stream data, char separator)
            => Converters.DataTableConverter.FromStream(data, separator);
        #endregion

        /// <summary>
        /// Convertir un objeto en un JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ObjectToJSON(object o)
            => o.ToJson();

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ObjectToData")]
        public static DataTable ObjectToData(object o)
            => Converters.DataManagement.ObjectToData(o);

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ObjectToData")]
        public DataTable ConvertObjectToData(object o)
            => Converters.DataManagement.ObjectToData(o);

        /// <summary>
        /// Obtener el DataSet de un DataSource
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataSet DataSouce2DataSet(object source)
        {
            try
            {
                return source as DataSet;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Obtener el DataTable de un DataSource
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable DataSouce2DataTable(object source)
        {
            try
            {
                DataTable dt;
                switch (Objetos.GetTipo(source).ToLower())
                {
                    case "dataset":
                        dt = DataSouce2DataSet(source).Tables[0];
                        break;
                    case "datatable":
                        dt = source as DataTable;
                        break;
                    case "dataview":
                        DataView dv = source as DataView;
                        dt = dv.ToTable();
                        break;
                    default:
                        dt = new DataTable();
                        break;
                }
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
            => dt.ToList<T>();

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertListToDataTable")]
        public DataTable ConvertListToDataTable(List<object> filas, string[] columnas)
            => Converters.DataManagement.ConvertListToDataTable(filas, columnas);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertListToDataTable")]
        public static DataTable ListToDataTable(List<object> filas, string[] columnas)
            => Converters.DataManagement.ConvertListToDataTable(filas, columnas);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="data">Datos que va a contener la lista.</param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertListToDataTable")]
        public static DataTable ListToDataTable<T>(List<T> data)
            => Converters.DataManagement.ConvertListToDataTable(data);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="data">Datos que va a contener la lista.</param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertListToDataTable")]
        public DataTable ConvertListToDataTable<T>(List<T> data)
            => ListToDataTable(data);

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertDataSetToJSON")]
        public string DatatableToJSON(DataSet ds)
            => Converters.DataManagement.ConvertDataSetToJSON(ds);

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertDataSetToJSON")]
        public static string DataSetToJSON(DataSet ds)
            => Converters.DataManagement.ConvertDataSetToJSON(ds);

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ConvertDatatableToJSON(DataTable dt)
            => dt.ToJson();

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DatatableToJSON(DataTable dt)
            => dt.ToJson();

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.JsonStringToDataTable")]
        public static DataTable JsonStringToDataTable(string jsonString)
            => Converters.DataManagement.JsonStringToDataTable(jsonString);

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        [Obsolete(message: "Use drualcman.Converters.DataManagement.ConvertJsonStringToDataTable")]
        public DataTable ConvertJsonStringToDataTable(string jsonString)
            => Converters.DataManagement.JsonStringToDataTable(jsonString);
    }
}