using System.Data;
using System.Xml;

namespace drualcman.Converters
{
    public class XmlConverter
    {
        public readonly XmlDocument Document;
        public XmlConverter(string data)
        {
            Document = new XmlDocument();
            Document.LoadXml(data);
        }

        #region object methods
        public XmlNodeList GetNodeByName(string nodeName) =>
            Document.GetElementsByTagName(nodeName);

        public string[] GetNodeInnetText(string nodeName)
        {
            List<string> result = new();
            foreach(XmlElement item in GetNodeByName(nodeName))
            {
                XmlNodeList nodes = GetNodeList(item);
                if(nodes is not null)
                {
                    foreach(XmlNode node in nodes)
                        result.Add(node.InnerText);
                }
            }
            return result.ToArray();
        }

        public XmlNodeList GetNodeList(XmlElement item)
        {
            XmlNodeList xLista = null;
            if(item.ChildNodes.Count > 0)
                xLista = item.ChildNodes;
            return xLista;
        }

        public IDictionary<string, string> ElementToDictionary(XmlElement item)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if(item.NodeType != XmlNodeType.XmlDeclaration)
            {
                if(item.ChildNodes.Count == 1 &&
                   item.Attributes.Count > 0)
                    for(int i = 0; i < item.Attributes.Count; i++)
                        result.Add(item.Attributes[i].Name, item.Attributes[i].Value);
            }
            return result;
        }

        public IList<XmlNode> GetNoteList()
        {
            List<XmlNode> nodes = new();
            foreach(XmlNode padre in Document)
                if(padre.NodeType != XmlNodeType.XmlDeclaration)
                    nodes.Add(padre);
            return nodes;
        }
        #endregion


        #region static methods
        /// <summary>
        /// Convert XML String in DataSet
        /// </summary>
        /// <param name="xml">Datos en formato XML</param>
        /// <returns></returns>
        public static DataSet ToDataSet(string xmlData)
        {
            DataSet ds = new DataSet();
            ds.DataSetName = "DataSet";
            XmlConverter xmlConverter = new XmlConverter(xmlData);
            foreach(XmlNode padre in xmlConverter.GetNoteList())
            {
                XmlNodeList xLista = padre.ChildNodes;
                GetNodos(xLista, ref ds, padre.Name);
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
            bool creaPadre = !ds.Tables.Contains(tabla);
            if(creaPadre) ds.Tables.Add(CreaTabla(xLista, tabla));
            else
            {
                DataTable tmp = ActualizaTabla(ds.Tables[tabla], xLista, tabla);
                ds.Tables[tabla].Reset();
                ds.Tables[tabla].Merge(tmp, false, MissingSchemaAction.Add);
            }

            DataTable dt = ds.Tables[tabla];
            DataRow dr = dt.NewRow();
            foreach(XmlElement item in xLista)
            {
                if(item.NodeType != XmlNodeType.XmlDeclaration)
                {
                    if(item.ChildNodes.Count > 1)
                    {
                        dr[item.Name] = item.Name + "[" + item.ChildNodes.Count.ToString() + "]";
                        //sobrecarga para comprobar de nuevo el nodo
                        XmlNodeList xLista1 = item.ChildNodes;
                        GetNodos(xLista1, ref ds, item.Name);
                    }
                    else
                    {
                        if(item.Attributes.Count > 0)
                        {
                            string content = dr[item.Name].ToString();
                            if(!string.IsNullOrEmpty(content)) content += "~";
                            for(int i = 0; i < item.Attributes.Count; i++)
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

        /// <summary>
        /// Crear la tabla con las columnas del nodo XML
        /// </summary>
        /// <param name="xLista">Nodo XML</param>
        /// <param name="tabla">Nombre del nodo para la tabla</param>
        /// <returns></returns>
        private static DataTable CreaTabla(XmlNodeList xLista, string tabla)
        {
            DataTable dt = new DataTable(tabla);
            foreach(XmlElement item in xLista)
            {
                if(!dt.Columns.Contains(item.Name))
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
            DataTable dt = new DataTable();
            dt.TableName = "ToMerge";
            foreach(XmlElement item in xLista)
            {
                if(!Original.Columns.Contains(item.Name))
                {
                    actualizar = true;
                    dt.Columns.Add(new DataColumn(item.Name));
                }
            }
            if(actualizar) Original.Merge(dt, true, MissingSchemaAction.Add);
            return Original;
        }
        #endregion

        #region async
        public static Task<DataSet> ToDataSetAsync(string xmlData)
            => Task.FromResult(ToDataSet(xmlData));
        #endregion
    }
}
