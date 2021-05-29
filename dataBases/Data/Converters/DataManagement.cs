using drualcman.Data.Extensions;
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
            public static DataTable ObjectToData(object o)
            {
                //https://stackoverflow.com/questions/18746064/using-reflection-to-create-a-datatable-from-a-class
                DataTable dt = new DataTable("OutputData");

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                o.GetType().GetProperties().ToList().ForEach(f =>
                {
                    try
                    {
                        f.GetValue(o, null);
                        dt.Columns.Add(f.Name, f.PropertyType);
                        dt.Rows[0][f.Name] = f.GetValue(o, null);
                    }
                    catch { }
                });
                return dt;
            }


            /// <summary>
            /// Convertir un objeto en un JSON
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            [Obsolete(message: "Use Object extension or drualcman.Data.Converters.ObjectConverter.ToJson")]
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
            public static DataTable ConvertListToDataTable(List<object> filas, string[] columnas)
            {
                //original http://stackoverflow.com/questions/18100783/how-to-convert-a-list-into-data-table

                // New table.
                DataTable table = new DataTable();
                table.TableName = "Tabla";

                // Add columns.
                for (int i = 0; i < columnas.Length; i++)
                {
                    table.Columns.Add(columnas[i]);
                }
                // Add rows.
                foreach (var array in filas)
                {

                    table.Rows.Add(array);
                }
                return table;
            }

            /// <summary>
            /// convert object into string delimeted by ;
            /// </summary>
            /// <param name="o"></param>
            /// <returns></returns>
            public static string GetPropiedades(Object o)
            {
                Type t = o.GetType();
                System.Reflection.PropertyInfo[] pis = t.GetProperties();
                string[] valores = pis.Select(p => p.Name + " : " + p.GetValue(o)).ToArray();
                string delimitados = string.Join(";", valores);
                return delimitados;
            }

            /// <summary>
            /// Convertir un List en un Datatable
            /// </summary>
            /// <param name="filas">Datos que va a contener la lista.</param>
            /// <param name="columnas">Nombres de las columnas para la tabla</param>
            /// <returns></returns>
            public static DataTable ConvertListToDataTable<T>(List<T> data)
            {
                // http://stackoverflow.com/questions/19076034/how-to-fill-a-datatable-with-listt
                System.ComponentModel.PropertyDescriptorCollection props =
                    System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                table.TableName = "Tabla";
                for (int i = 0; i < props.Count; i++)
                {
                    System.ComponentModel.PropertyDescriptor prop = props[i];
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                    else
                    {
                        if (prop.PropertyType.IsArray)
                        {
                            table.Columns.Add(prop.Name, typeof(String));
                        }
                        else table.Columns.Add(prop.Name, prop.PropertyType);

                    }
                }
                object[] values = new object[props.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (props[i].PropertyType.IsArray)
                        {
                            values[i] = GetPropiedades(props[i].GetValue(item));
                            if (values[i].ToString().ToLower().IndexOf("length : 0") >= 0) values[i] = string.Empty;
                        }
                        else values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }

            /// <summary>
            /// Crear una lista generica desde un DataTable
            /// </summary>
            /// <typeparam name="T">Nombre de la clase a recibir los datos</typeparam>
            /// <param name="tbl">Tabla contenedora de los datos</param>
            /// <returns></returns>
            public static List<T> ConvertDataTableToList<T>(DataTable tbl) where T : new()
            {
                //https://stackoverflow.com/questions/8008389/how-to-convert-datatable-to-class-object
                // define return list
                List<T> lst = new List<T>();

                // go through each row
                foreach (DataRow r in tbl.Rows)
                {
                    // add to the list
                    lst.Add(CreateItemFromRow<T>(r));
                }

                // return the list
                return lst;
            }

            /// <summary>
            /// Convertir uyn DataSet en un objero JSON
            /// </summary>
            /// <param name="ds"></param>
            /// <returns></returns>
            public static string ConvertDataSetToJSON(DataSet ds)
            {
                //https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp                
                System.Text.StringBuilder jsonString = new System.Text.StringBuilder();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    jsonString.Append("[");
                    for (int rows = 0; rows < ds.Tables[0].Rows.Count; rows++)
                    {
                        jsonString.Append("{");
                        for (int cols = 0; cols < ds.Tables[0].Columns.Count; cols++)
                        {
                            jsonString.Append(@"""" + ds.Tables[0].Columns[cols].ColumnName + @""":");
                        }
                        jsonString.Append(rows == ds.Tables[0].Rows.Count - 1 ? "}" : "},");
                    }
                    jsonString.Append("]");
                    return jsonString.ToString();
                }
                return null;
            }

            public static string ConvertDatatableToJSON(DataSet ds)
            {
                return dataBases.DataManagement.ConvertDataSetToJSON(ds);
            }

            /// <summary>
            /// Convert DataTable to Json
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            [Obsolete(message: "Use DataTable Extension method or drualcman.Data.Converters.DataTableConverter.ToJson")]
            public static string ConvertDataTableToJSON(DataTable dt)
            {
                return drualcman.Data.Converters.DataTableConverter.ToJson(dt);
            }

            #region Helpers          
            /// <summary>
            /// Check don't have a , between " on the text.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            private static string CheckComa(string text)
            {
                string retorno = string.Empty;
                string caracter;
                string siguiente;
                for (int i = 0; i < text.Length; i++)
                {
                    caracter = text.Substring(i, 1);
                    int n = i + 1;
                    if (n < text.Length)
                    {
                        siguiente = text.Substring(n, 1);
                        if (caracter == "," && siguiente != "\"")
                        {
                            if (caracter == "," && siguiente != "{") retorno += string.Empty;
                            else if (caracter == "," && siguiente == " ")
                            {
                                n++;
                                siguiente = text.Substring(n, 1);
                                if (caracter == "," && siguiente != "\"")
                                {
                                    if (caracter == "," && siguiente != "{") retorno += string.Empty;
                                    else retorno += caracter;
                                }
                                else retorno += caracter;
                            }
                            else retorno += caracter;
                        }
                        else retorno += caracter;
                    }
                    else retorno += caracter;
                }
                return retorno;
            }

            public static DataTable JsonStringToDataTable(string jsonString)
            {
                //http://www.c-sharpcorner.com/blogs/convert-json-string-to-datatable-in-asp-net1
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(jsonString) && jsonString.ToLower() != "undefined")
                {
                    jsonString = jsonString.Replace("}, {", "},{");
                    jsonString = CheckComa(jsonString);
                    string[] jsonStringArray = System.Text.RegularExpressions.Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                    List<string> ColumnsName = new List<string>();
                    foreach (string jSA in jsonStringArray)
                    {
                        string[] jsonStringData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                        foreach (string ColumnsNameData in jsonStringData)
                        {
                            try
                            {
                                int idx = ColumnsNameData.IndexOf(":");
                                string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "").Trim();
                                if (!ColumnsName.Contains(ColumnsNameString))
                                {
                                    ColumnsName.Add(ColumnsNameString);
                                }
                                else
                                {
                                    //if found more than one column with same name add the id to difference the column name
                                    ColumnsName.Add(ColumnsNameString + (ColumnsName.Count() - 1).ToString());
                                }
                            }
                            catch
                            {
                                throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                            }
                        }
                        break;
                    }
                    foreach (string AddColumnName in ColumnsName)
                    {
                        dt.Columns.Add(AddColumnName);
                    }
                    foreach (string jSA in jsonStringArray)
                    {
                        string[] RowData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                        DataRow nr = dt.NewRow();
                        int columnNumber = 0;       //reset index of the column per each element
                        foreach (string rowData in RowData)
                        {
                            try
                            {
                                int idx = rowData.IndexOf(":");
                                string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                                nr[columnNumber] = RowDataString;       //because the columns always come in same order use the index not the name
                                columnNumber++;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        dt.Rows.Add(nr);
                    }
                }
                return dt;
            }

            public static List<Dictionary<string, string>> JsonStringToListDictionary(string jsonString)
            {
                List<Dictionary<string, string>> dt = new List<Dictionary<string, string>>();
                if (!string.IsNullOrEmpty(jsonString) && jsonString.ToLower() != "undefined")
                {
                    jsonString = jsonString.Replace("}, {", "},{");
                    jsonString = CheckComa(jsonString);
                    string[] jsonStringArray = System.Text.RegularExpressions.Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                    List<string> ColumnsName = new List<string>();
                    foreach (string jSA in jsonStringArray)
                    {
                        string[] jsonStringData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                        foreach (string rowData in jsonStringData)
                        {
                            try
                            {
                                int idx = rowData.IndexOf(":");
                                string ColumnsNameString = rowData.Substring(0, idx - 1).Replace("\"", "").Trim();
                                if (!ColumnsName.Contains(ColumnsNameString))
                                {
                                    ColumnsName.Add(ColumnsNameString);
                                }
                                else
                                {
                                    //if found more than one column with same name add the id to difference the column name                                    
                                    ColumnsName.Add(ColumnsNameString = ColumnsNameString + (ColumnsName.Count() - 1).ToString());
                                }
                            }
                            catch
                            {
                                throw new Exception(string.Format("Error Parsing Column Name : {0}", rowData));
                            }
                        }
                        break;
                    }
                    foreach (string jSA in jsonStringArray)
                    {
                        
                        string[] RowData = System.Text.RegularExpressions.Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");                        
                        int columnNumber = 0;       //reset index of the column per each element
                        Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                        foreach (string rowData in RowData)
                        {
                            try
                            {
                                int idx = rowData.IndexOf(":");
                                string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                                valuePairs.Add(ColumnsName[columnNumber], RowDataString);
                                columnNumber++;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        dt.Add(valuePairs);
                    }
                }
                return dt;
            }


            // function that creates an object from the given data row
            private static T CreateItemFromRow<T>(DataRow row) where T : new()
            {
                // create a new object
                T item = new T();

                // set the item
                SetItemFromRow(item, row);

                // return 
                return item;
            }

            private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
            {
                // go through each column
                foreach (DataColumn c in row.Table.Columns)
                {
                    // find the property for the column
                    System.Reflection.PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                    // if exists, set the value
                    if (p != null && row[c] != DBNull.Value)
                    {
                        p.SetValue(item, row[c], null);
                    }
                }
            }
            #endregion
        }
    }
}