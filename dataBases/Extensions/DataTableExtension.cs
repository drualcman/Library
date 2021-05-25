using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Extensions
{
    public static class DataTableExtension
    {
        #region methods
        #region TO
        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <param name="columns">columns name to parse</param>
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(this DataTable dt, string[] columns) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                List<TModel> result = new List<TModel>();
                PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                TModel item = new TModel();
                string[] rowCols = dt.Rows[0].ColumnNamesToArray();
                
                List<PropertyInfo> columnas = new List<PropertyInfo>();
                int p = properties.Length;

                for (int i = 0; i < p; i++)
                {
                    if (columns.Contains(properties[i].Name, StringComparer.OrdinalIgnoreCase) &&
                       rowCols.Contains(properties[i].Name, StringComparer.OrdinalIgnoreCase))
                    {
                        columnas.Add(properties[i]);
                    }
                }

                int c = dt.Rows.Count;
                for (int x = 0; x < c; x++)
                {
                    bool hasData = false;
                    for (int i = 0; i < p; i++)
                    {
                        try
                        {
                            properties[i].SetValue(item, dt.Rows[x][properties[i].Name], null);
                            hasData = true;
                        }
                        catch
                        {

                        }
                    }
                    if (hasData)
                        result.Add(item);      //only add the item if have some to add
                }
                return result;
            }
            else return new List<TModel>();          //no results
        }

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(this DataTable dt) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                string[] columns = dt.ColumnNamesToArray();                
                return  dt.ToList<TModel>(columns);
            }
            else return new List<TModel>();          //no results
        }

        /// <summary>
        /// Convert DataTable to Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            //https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp                
            StringBuilder jsonString = new StringBuilder();
            int r = dt.Rows.Count;
            int c = dt.Columns.Count;
            if (r > 0)
            {
                jsonString.Append("[");
                for (int row = 0; row < r; row++)
                {
                    jsonString.Append("{");
                    for (int col = 0; col < c; col++)
                    {
                        jsonString.Append(@"""" + dt.Columns[col].ColumnName + @""":");

                        bool last = !(col < c - 1);
                        jsonString.Append(GenerateJsonProperty(dt, row, col, last));
                    }
                    jsonString.Append(row == dt.Rows.Count - 1 ? "}" : "},");
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region FROM
        /// <summary>
        /// Parse DataTable from Stream Data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static DataTable FromStream(this DataTable dt, Stream data, char separator)
        {
            StreamReader sr = new StreamReader(data);
            string[] headers = sr.ReadLine().Split(separator);
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

        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static DataTable FromFile(this DataTable dt, string filePath, char separator)
        {
            StreamReader sr = new StreamReader(filePath);
            return dt.FromStream(sr.BaseStream, separator);
        }

        /// <summary>
        /// Parse JsonString to datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static DataTable FromJson(this DataTable dt, string jsonString)
        {
            //http://www.c-sharpcorner.com/blogs/convert-json-string-to-datatable-in-asp-net1
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
        #endregion
        #endregion

        #region async
        #region TO
        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<List<TModel>> ToListAsync<TModel>(this DataTable dt, string[] columns) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                return Task.FromResult(dt.ToList<TModel>(columns));
            }
            else return Task.FromResult(new List<TModel>());          //no results
        }

        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<List<TModel>> ToListAsync<TModel>(this DataTable dt) where TModel : new()
        {
            if (dt.Rows.Count > 0)
            {
                string[] columns = await dt.ColumnNamesToArrayAsync();
                return await dt.ToListAsync<TModel>(columns);
            }
            else return new List<TModel>();          //no results
        }

        /// <summary>
        /// Convertir uyn DataTable en un objero JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<string> ToJsonAsync(this DataTable dt)
            => Task.FromResult(ToJson(dt));
        #endregion

        #region FROM

        /// <summary>
        /// Parse DataTable from Stream Data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        public static async Task<DataTable> FromStreamAsync(this DataTable dt, Stream data, char separator)
        {
            StreamReader sr = new StreamReader(data);
            string head = await sr.ReadLineAsync();
            string[] headers = head.Split(separator);
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string rowHead = await sr.ReadLineAsync();
                string[] rows = rowHead.Split(separator);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Convert from csv file into a DataTable
        /// </summary>
        /// <param name="filePath">full path to get the file to part into a datatable</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static async Task<DataTable> FromFileAsync(this DataTable dt, string filePath, char separator)
        {
            StreamReader sr = new StreamReader(filePath);
            return await dt.FromStreamAsync(sr.BaseStream, separator);
        }

        /// <summary>
        /// Parse JsonString to datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Task<DataTable> FromJsonAsync(this DataTable dt, string jsonString)
            => Task.FromResult(FromJson(dt, jsonString));
        #endregion
        #endregion


        #region Helpers
        private static string GenerateJsonProperty(DataTable dt, int row, int col, bool isLast = false)
        {
            // IF LAST PROPERTY THEN REMOVE 'COMMA'  IF NOT LAST PROPERTY THEN ADD 'COMMA'
            string addComma = isLast ? "" : ",";
            numeros n = new numeros();
            StringBuilder jsonString = new StringBuilder();
            if (dt.Rows[row][col] == DBNull.Value)
            {
                jsonString.Append(" null " + addComma);
            }
            else if (dt.Columns[col].DataType == typeof(DateTime))
            {
                jsonString.Append(@"""" + (((DateTime)dt.Rows[row][col]).ToString("yyyy-MM-dd HH':'mm':'ss")) + @"""" + addComma);
            }
            else if (dt.Columns[col].DataType == typeof(string))
            {
                jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
            }
            else if (dt.Columns[col].DataType == typeof(bool))
            {
                jsonString.Append(Convert.ToBoolean(dt.Rows[row][col]) ? "true" + addComma : "false" + addComma);
            }
            else if (dt.Columns[col].DataType == typeof(Int16))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Int32))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true, "{0:0}") + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Int64))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true, "{0:0}") + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(int))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true, "{0:0}") + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Double))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Decimal))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(float))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(long))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(short))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Single))
            {
                try
                {
                    jsonString.Append(n.number2String(dt.Rows[row][col], true) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else if (dt.Columns[col].DataType == typeof(Byte))
            {
                try
                {
                    jsonString.Append(Convert.ToByte(dt.Rows[row][col]) + addComma);
                }
                catch
                {
                    jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
                }
            }
            else
            {
                jsonString.Append(@"""" + (dt.Rows[row][col].ToString().Replace("\"", "\\\"")) + @"""" + addComma);
            }
            return jsonString.ToString();
        }

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

        #endregion
    }
}
