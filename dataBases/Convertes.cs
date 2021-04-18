using drualcman.Data.Converters;
using drualcman.Data.Extensions;
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
        public static string GetCSV(DataView dv)
            => CSVConverter.GetCSV(dv, ",");

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataView dt, string separator)
            => CSVConverter.GetCSV(dt, separator);

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataTable dt)
            => CSVConverter.GetCSV(dt, ",");

        /// <summary>
        /// get a CSV file
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="separator">Separator to use</param>
        /// <returns></returns>
        public static string GetCSV(DataTable dt, string separator)
            => CSVConverter.GetCSV(dt, separator);

        public static DataTable CSVtoData(Stream data, char separator)
            => DataTableConverter.FromStream(data, separator);
        #endregion

        /// <summary>
        /// Convertir un objeto en un JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ObjectToJSON(object o)
            => DataManagement.ObjectToJSON(o);

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DataTable ObjectToData(object o)
            => DataManagement.ObjectToData(o);

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public DataTable ConvertObjectToData(object o)
            => dataBases.ObjectToData(o);

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
            catch (Exception ex)
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
                switch (utilidades.getTipo(source).ToLower())
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
            catch (Exception ex)
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
        public DataTable ConvertListToDataTable(List<object> filas, string[] columnas)
            => DataManagement.ConvertListToDataTable(filas, columnas);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public static DataTable ListToDataTable(List<object> filas, string[] columnas)
            => DataManagement.ConvertListToDataTable(filas, columnas);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> data) 
            => DataManagement.ConvertListToDataTable(data);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public DataTable ConvertListToDataTable<T>(List<T> data) 
            => ListToDataTable(data);

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string DatatableToJSON(DataSet ds)
            => DataManagement.ConvertDataSetToJSON(ds);

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToJSON(DataSet ds)
            => DataManagement.ConvertDataSetToJSON(ds);

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ConvertDatatableToJSON(DataTable dt)
            => DataManagement.ConvertDataTableToJSON(dt);

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DatatableToJSON(DataTable dt)
            => DataManagement.ConvertDataTableToJSON(dt);

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static DataTable JsonStringToDataTable(string jsonString)
            => DataManagement.JsonStringToDataTable(jsonString);

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public DataTable ConvertJsonStringToDataTable(string jsonString)
            => DataManagement.JsonStringToDataTable(jsonString);
    }
}