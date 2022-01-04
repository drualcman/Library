using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace drualcman.Data.Converters
{
    public class JsonConverter
    {
        /// <summary>
        /// Convertir un objeto a data table
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ObjectToData")]
        public static DataTable ObjectToData(object o) =>
            drualcman.Converters.JsonConverter.ObjectToData(o);

        /// <summary>
        /// Convertir un objeto en un JSON
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ObjectToJSON")]
        public static string ObjectToJSON(object o) =>
            drualcman.Converters.JsonConverter.ObjectToJSON(o);

        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.xml2dataset")]
        public static DataSet xml2dataset(string xmlData) =>
            drualcman.Converters.JsonConverter.xml2dataset(xmlData);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="filas">Datos que va a contener la lista.</param>
        /// <param name="columnas">Nombres de las columnas para la tabla</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertListToDataTable")]
        public static DataTable ConvertListToDataTable(List<object> filas, string[] columnas) =>
            drualcman.Converters.JsonConverter.ConvertListToDataTable(filas, columnas);

        /// <summary>
        /// convert object into string delimeted by ;
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataManagement.GetPropiedades")]
        public static string GetPropiedades(Object o) =>
            drualcman.Converters.DataManagement.GetPropiedades(o);

        /// <summary>
        /// Convertir un List en un Datatable
        /// </summary>
        /// <param name="data">Datos que va a contener la lista.</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertListToDataTable")]
        public static DataTable ConvertListToDataTable<T>(List<T> data) =>
            drualcman.Converters.JsonConverter.ConvertListToDataTable(data);

        /// <summary>
        /// Crear una lista generica desde un DataTable
        /// </summary>
        /// <typeparam name="T">Nombre de la clase a recibir los datos</typeparam>
        /// <param name="tbl">Tabla contenedora de los datos</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertDataTableToList")]
        public static List<T> ConvertDataTableToList<T>(DataTable tbl) where T : new() =>
            drualcman.Converters.JsonConverter.ConvertDataTableToList<T>(tbl);

        /// <summary>
        /// Convertir uyn DataSet en un objero JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertDataSetToJSON")]
        public static string ConvertDataSetToJSON(DataSet ds) =>
            drualcman.Converters.JsonConverter.ConvertDataSetToJSON(ds);

        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertDatatableToJSON")]
        public static string ConvertDatatableToJSON(DataSet ds) =>
            drualcman.Converters.JsonConverter.ConvertDataSetToJSON(ds);

        /// <summary>
        /// Convertir uyn DataTable en un objero JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.ConvertDataTableToJSON")]
        public static string ConvertDataTableToJSON(DataTable dt) =>
            drualcman.Converters.JsonConverter.ConvertDataTableToJSON(dt);

        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.JsonConverter.JsonStringToDataTable")]
        public static DataTable JsonStringToDataTable(string jsonString) =>
            drualcman.Converters.JsonConverter.JsonStringToDataTable(jsonString);
    }
}
