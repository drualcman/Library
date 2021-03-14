using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

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
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataView dt)
        {
            return GetCSV(dt, ",");
        }

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataView dt, string separator)
        {
            return GetCSV(dt.ToTable(), separator);
        }

        /// <summary>
        /// get a CSV file with a , separator
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetCSV(DataTable dt)
        {
            return GetCSV(dt, ",");
        }

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

        public static DataTable CSVtoData(Stream data, char separator)
        {
            StreamReader sr = new StreamReader(data);
            string[] headers = sr.ReadLine().Split(separator);
            DataTable dt = new DataTable();
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

        /// <summary>
        /// Convertir un objeto en un JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ObjectToJSON(object o)
        {
            return DataManagement.ObjectToJSON(o);
        }

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DataTable ObjectToData(object o)
        {
            return DataManagement.ObjectToData(o);
        }

        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public DataTable ConvertObjectToData(object o)
        {
            return dataBases.ObjectToData(o);
        }

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
                throw ex;
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
                throw ex;
            }
        }

        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (System.Reflection.PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        } 

        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        public static DataSet xml2dataset(string xmlData)
        {
            return DataManagement.xml2dataset(xmlData);
        }

        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        public DataSet ConvertXml2dataset(string xmlData)
        {
            return DataManagement.xml2dataset(xmlData);
        }

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public DataTable ConvertListToDataTable(List<object> filas, string[] columnas)
        {
            return DataManagement.ConvertListToDataTable(filas, columnas);
        }

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public static DataTable ListToDataTable(List<object> filas, string[] columnas)
        {
            return DataManagement.ConvertListToDataTable(filas, columnas);
        }

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> data)
        {
            return DataManagement.ConvertListToDataTable<T>(data);
        }

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        public DataTable ConvertListToDataTable<T>(List<T> data)
        {
            return DataManagement.ConvertListToDataTable<T>(data);
        }

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string DatatableToJSON(DataSet ds)
        {
            return DataManagement.ConvertDataSetToJSON(ds);
        }

        /// <summary>
        /// Convertir un dataset en JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToJSON(DataSet ds)
        {
            return DataManagement.ConvertDataSetToJSON(ds);
        }

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ConvertDatatableToJSON(DataTable dt)
        {
            return DataManagement.ConvertDataTableToJSON(dt);
        }

        /// <summary>
        /// Convertir un data table en JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DatatableToJSON(DataTable dt)
        {
            return DataManagement.ConvertDataTableToJSON(dt);
        }

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static DataTable JsonStringToDataTable(string jsonString)
        {
            return DataManagement.JsonStringToDataTable(jsonString);
        }

        /// <summary>
        /// Convert JSON data format in DataTable
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public DataTable ConvertJsonStringToDataTable(string jsonString)
        {
            return DataManagement.JsonStringToDataTable(jsonString);
        }
    }
}