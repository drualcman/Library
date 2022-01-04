using System;
using System.Data;
using System.Threading.Tasks;

namespace drualcman.Data.Converters
{
    public class XmlConverter
    {
        #region methods
        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.XmlConverter.ToDataSet")]
        public static DataSet ToDataSet(string xmlData) =>
            drualcman.Converters.XmlConverter.ToDataSet(xmlData);
        #endregion

        #region async
        [Obsolete(message: "Use DataTable Extension method or drualcman.Converters.XmlConverter.ToDataSetAsync")]
        public static Task<DataSet> ToDataSetAsync(string xmlData)
            => Task.FromResult(ToDataSet(xmlData));
        #endregion
    }

    public partial class dataBases
    {
        /// <summary>
        /// Manejo de datos
        /// </summary>
        public partial class DataManagement
        {
            /// <summary>
            /// Convert XML String in DataSet
            /// </summary>
            /// <param name="xml">Datos en formato XML</param>
            /// <returns></returns>
            [Obsolete(message: "Use drualcman.Data.Converters.XmlConverter.ToDataSet")]
            public static DataSet xml2dataset(string xmlData)
            {
                return XmlConverter.ToDataSet(xmlData);
            }
        }
    }
}
