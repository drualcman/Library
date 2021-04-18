using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace drualcman.Data.Converters
{
    public class XmlConverter
    {
        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        public static DataSet xml2dataset(string xmlData)
        {
            return dataBases.DataManagement.xml2dataset(xmlData);
        }

        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        public DataSet ConvertXml2dataset(string xmlData)
        {
            return dataBases.DataManagement.xml2dataset(xmlData);
        }
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
            public static DataSet xml2dataset(string xmlData)
            {
                DataSet ds = new DataSet();
                ds.DataSetName = "DataSet";
                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlData);
                    foreach (XmlNode padre in xDoc)
                    {
                        if (padre.NodeType != XmlNodeType.XmlDeclaration)
                        {
                            XmlNodeList xLista = padre.ChildNodes;
                            GetNodos(xLista, ref ds, padre.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return ds;
            }

            /// <summary>
            /// Recorer el nodo para extraer los datos
            /// </summary>
            /// <param name="xLista">Nodo XML</param>
            /// <param name="ds">Dataset a agregar las tablas</param>
            /// <param name="tabla">Nombre del nodo para la tabla</param>
            private static void GetNodos(XmlNodeList xLista, ref DataSet ds, string tabla)
            {
                try
                {
                    bool creaPadre = !ds.Tables.Contains(tabla);
                    if (creaPadre) ds.Tables.Add(CreaTabla(xLista, tabla));
                    else ActualizaTabla(ds.Tables[tabla], xLista, tabla);

                    DataTable dt = ds.Tables[tabla];
                    dt.TableName = "Tabla";
                    DataRow dr = dt.NewRow();
                    foreach (XmlElement item in xLista)
                    {
                        if (item.NodeType != XmlNodeType.XmlDeclaration)
                        {
                            if (item.ChildNodes.Count > 1)
                            {
                                dr[item.Name] = item.Name + "[" + item.ChildNodes.Count.ToString() + "]";
                                //sobrecarga para comprobar de nuevo el nodo
                                XmlNodeList xLista1 = item.ChildNodes;
                                GetNodos(xLista1, ref ds, item.Name);
                            }
                            else
                            {
                                if (item.Attributes.Count > 0)
                                {
                                    string content = dr[item.Name].ToString();
                                    if (!string.IsNullOrEmpty(content)) content += "~";
                                    for (int i = 0; i < item.Attributes.Count; i++)
                                    {
                                        content += item.Attributes[i].Name;
                                        content += ":";
                                        content += item.Attributes[i].InnerText;
                                        content += ";";
                                    }
                                    dr[item.Name] = content;
                                }
                                else dr[item.Name] = item.InnerText;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                    ds.Tables.Remove(tabla);
                    ds.Tables.Add(dt);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            /// <summary>
            /// Crear la tabla con las columnas del nodo XML
            /// </summary>
            /// <param name="xLista">Nodo XML</param>
            /// <param name="tabla">Nombre del nodo para la tabla</param>
            /// <returns></returns>
            private static DataTable CreaTabla(XmlNodeList xLista, string tabla)
            {
                DataTable dt = new DataTable(tabla);
                dt.TableName = "Tabla";
                foreach (XmlElement item in xLista)
                {
                    if (!dt.Columns.Contains(item.Name))
                    {
                        dt.Columns.Add(new DataColumn(item.Name));
                    }
                }
                return dt;
            }

            /// <summary>
            /// Actualizar las columnas de la tabla con las columnas del nodo XML
            /// </summary>
            /// <param name="Original">Tabla Original</param>
            /// <param name="xLista">Nodo XML</param>
            /// <param name="tabla">Nombre del nodo para la tabla</param>
            /// <returns></returns>
            private static DataTable ActualizaTabla(DataTable Original, XmlNodeList xLista, string tabla)
            {
                bool actualizar = false;
                DataTable dt = new DataTable(tabla);
                dt.TableName = "Tabla";
                foreach (XmlElement item in xLista)
                {
                    if (!Original.Columns.Contains(item.Name))
                    {
                        actualizar = true;
                        dt.Columns.Add(new DataColumn(item.Name));
                    }
                }
                if (actualizar) Original.Merge(dt, true, MissingSchemaAction.Add);
                return Original;
            }

        }
    }
}
