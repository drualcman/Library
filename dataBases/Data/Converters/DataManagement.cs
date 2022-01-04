using drualcman.Converters.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace drualcman
{
    public partial class dataBases
    {
        /// <summary>
        /// Manejo de datos
        /// </summary>
        public partial class DataManagement
        {
            /// <summary>
            /// Convertir un objeto a data table
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ObjectToData")]
            public static DataTable ObjectToData(object o) => 
                Converters.DataManagement.ObjectToData(o);


            /// <summary>
            /// Convertir un objeto en un JSON
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ToJson")]
            public static string ObjectToJSON(object o)
            {
                return o.ToJson();
            }

            /// <summary>
            /// Convertir un List en un Datatable
            /// </summary>
            /// <param name="filas">Datos que va a contener la lista.</param>
            /// <param name="columnas">Nombres de las columnas para la tabla</param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ConvertListToDataTable")]
            public static DataTable ConvertListToDataTable(List<object> filas, string[] columnas) =>
                Converters.DataManagement.ConvertListToDataTable(filas, columnas);

            /// <summary>
            /// convert object into string delimeted by ;
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.GetPropiedades")]
            public static string GetPropiedades(Object o) =>
                Converters.DataManagement.GetPropiedades(o);

            /// <summary>
            /// Convertir un List en un Datatable
            /// </summary>
            /// <param name="data">Datos que va a contener la lista.</param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ConvertListToDataTable")]
            public static DataTable ConvertListToDataTable<T>(List<T> data) =>
                Converters.DataManagement.ConvertListToDataTable<T>(data);

            /// <summary>
            /// Crear una lista generica desde un DataTable
            /// </summary>
            /// <typeparam name="T">Nombre de la clase a recibir los datos</typeparam>
            /// <param name="tbl">Tabla contenedora de los datos</param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ConvertDataTableToList")]
            public static List<T> ConvertDataTableToList<T>(DataTable tbl) where T : new() =>
                Converters.DataManagement.ConvertDataTableToList<T>(tbl);

            /// <summary>
            /// Convertir uyn DataSet en un objero JSON
            /// </summary>
            /// <param name="ds"></param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ConvertDataSetToJSON")]
            public static string ConvertDataSetToJSON(DataSet ds) =>
                Converters.DataManagement.ConvertDataSetToJSON(ds);

            [Obsolete(message: "Use Object extension or drualcman.Converters.DataManagement.ConvertDatatableToJSON")]
            public static string ConvertDatatableToJSON(DataSet ds) =>
                Converters.DataManagement.ConvertDatatableToJSON(ds);

            /// <summary>
            /// Convert DataTable to Json
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.ConvertDataTableToJSON")]
            public static string ConvertDataTableToJSON(DataTable dt)
            {
                return Converters.DataTableConverter.ToJson(dt);
            }

            [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.JsonStringToDataTable")]
            public static DataTable JsonStringToDataTable(string jsonString) =>
                Converters.DataManagement.JsonStringToDataTable(jsonString);

            [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.DataTableConverter.JsonStringToListDictionary")]
            public static List<Dictionary<string, string>> JsonStringToListDictionary(string jsonString) =>
                Converters.DataManagement.JsonStringToListDictionary(jsonString);
        }
    }
}