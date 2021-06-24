﻿using drualcman.Attributes;
using drualcman.Enums;
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
        #region columns
        #region methods

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> ColumnNamesToList(this DataTable dt)
        {
            List<string> names = new List<string>();

            foreach (DataColumn item in dt.Columns)
            {
                names.Add(item.ColumnName);
            }

            return names;
        }

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string[] ColumnNamesToArray(this DataTable dt)
        {
            List<string> names = new List<string>();

            foreach (DataColumn item in dt.Columns)
            {
                names.Add(item.ColumnName);
            }

            return names.ToArray();
        }

        #endregion

        #region async
        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<List<string>> ColumnNamesToListAsync(this DataTable dt) => Task.FromResult(dt.ColumnNamesToList());

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Task<string[]> ColumnNamesToArrayAsync(this DataTable dt) => Task.FromResult(dt.ColumnNamesToArray());

        #endregion
        #endregion
        
        #region methods
        #region TO
        /// <summary>
        /// Get list of object send from data table
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dt"></param>
        /// <param name="columns">columns name to parse</param>
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(this DataTable dt, string[] columnNames) where TModel : new()
        {
            List<TModel> result = new List<TModel>();
            int r = dt.Rows.Count;
            if (r > 0)
            {
                Type model = typeof(TModel);

                List<TableName> tables = new List<TableName>();
                int tableCount = 0;
                TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(table);

                List<string> hasList = new List<string>();
                bool isDirectQuery = columnNames[0].IndexOf(".") < 0;
                List<Columns> columns = HaveColumns(columnNames, model, table.ShortName, true);

                int c = columns.Count;
                for (int i = 0; i < r; i++)
                {
                    result.Add((TModel)ColumnToObject(ref columnNames, dt.Rows[i], model, ref tables, ref tableCount, ref hasList, columns));
                }

                //if (hasList.Any())
                //{
                //    //need to create a list of object who is named in the list
                //    //1. create a list grouped by main model
                //    //List<TModel> mainModel = result.g;

                //}
            }
            return result;
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
                        jsonString.Append("\"");
                        jsonString.Append(dt.Columns[col].ColumnName);
                        jsonString.Append("\":");

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
        /// <summary>
        /// Get all the columns properties need from the query used
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="model"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        static private List<Columns> HaveColumns(string[] columns, Type model, string shortName, bool ignoreCase)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            int t = columns.Length;
            int p = properties.Length;

            bool isDirectQuery = columns[0].IndexOf(".") < 0;
            string columnCompare;
            for (int r = 0; r < t; r++)
            {
                int c = 0;
                bool have = false;
                DatabaseAttribute options = null;
                while (c < p && have == false)
                {
                    options = properties[c].GetCustomAttribute<DatabaseAttribute>();
                    if (isDirectQuery)
                    {
                        columnCompare = columns[r];
                    }
                    else
                    {
                        if (options is null)
                        {
                            columnCompare = columns[r].Replace($"{shortName}.", "");
                        }
                        else if (options.Inner == InnerDirection.NONE)
                        {
                            columnCompare = columns[r].Replace($"{shortName}.", "");
                        }
                        else
                        {
                            columnCompare = string.Empty;
                        }
                    }
                    if (ignoreCase)
                        have = columnCompare.ToLower() == properties[c].Name.ToLower();
                    else
                        have = columnCompare == properties[c].Name;
                    
                    c++;
                }
                if (have)
                {
                    result.Add(new Columns { Column = properties[c], Options = options, TableShortName = shortName, ColumnName = columns[r] });
                }
            }
            return result;
        }

        private static object ColumnToObject(ref string[] columnNames, DataRow row, Type model,
            ref List<TableName> tables, ref int tableCount, ref List<string> hasList, List<Columns> columns)
        {
            TableName table = tables.Where(t => t.Name == model.Name).FirstOrDefault();
            if (table is null)
            {
                tableCount++;
                table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(table);
            }
            if (columns.Where(t => t.TableShortName == table.ShortName).FirstOrDefault() == null)
            {
                columns = HaveColumns(columnNames, model, table.ShortName, true);
            }
            var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);

            string[] rowCols = row.ColumnNamesToArray();
            int c = columns.Count;
            for (int i = 0; i < c; i++)
            {
                if (!string.IsNullOrEmpty(columns[i].ColumnName)) 
                {
                    if (columns[i].Options is not null)
                    {
                        if (!columns[i].Options.Ignore)
                        {
                            if (Helpers.ObjectHelpers.IsGenericList(columns[i].Column.PropertyType.FullName) &&
                                            !hasList.Contains(columns[i].Column.PropertyType.Name))
                            {
                                hasList.Add(columns[i].Column.PropertyType.Name);
                                Type[] genericType = columns[i].Column.PropertyType.GetGenericArguments();
                                Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                                columns[i].Column.SetValue(item, Activator.CreateInstance(creatingCollectionType));
                            }
                            else if (columns[i].Options.Inner != InnerDirection.NONE)
                            {
                                try
                                {
                                    columns[i].Column.SetValue(item,
                                        ColumnToObject(ref columnNames, row, columns[i].Column.PropertyType, ref tables, ref tableCount, ref hasList, columns),
                                        null);
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                                        columns[i].Column.SetValue(item, Convert.ToBoolean(row[columns[i].ColumnName]), null);
                                    else
                                    {
                                        try
                                        {
                                            columns[i].Column.SetValue(item, row[columns[i].ColumnName], null);
                                        }
                                        catch (Exception ex)
                                        {
                                            //TODO some control
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        //sergi check
                        if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                        {
                            try
                            {
                                columns[i].Column.SetValue(item, Convert.ToBoolean(row[columns[i].ColumnName]), null);
                            }
                            catch
                            {
                                columns[i].Column.SetValue(item, false, null);
                            }
                        }
                        else
                        {
                            try
                            {
                                columns[i].Column.SetValue(item, row[columns[i].ColumnName], null);
                            }
                            catch (Exception ex)
                            {
                                //TODO some control
                                columns[i].Column.SetValue(item, ex.Message, null);
                            }
                        }
                        //if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                        //    columns[i].Column.SetValue(item, Convert.ToBoolean(row[columns[i].Column.Name]), null);
                        //else
                        //{
                        //    try
                        //    {
                        //        columns[i].Column.SetValue(item, row[columns[i].Column.Name], null);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        //TODO some control
                        //    }
                        //}
                    }
                }               
            }

            return item;
        }

        private static string GenerateJsonProperty(DataTable dt, int row, int col, bool isLast = false)
        {
            // IF LAST PROPERTY THEN REMOVE 'COMMA'  IF NOT LAST PROPERTY THEN ADD 'COMMA'
            string addComma = isLast ? "" : ",";
            StringBuilder jsonString = new StringBuilder();
            if (dt.Rows[row][col] == DBNull.Value)
            {
                jsonString.Append(" null ");
            }
            else if (dt.Columns[col].DataType == typeof(DateTime))
            {
                jsonString.Append("\"");
                jsonString.Append(((DateTime)dt.Rows[row][col]).ToString("yyyy-MM-dd HH':'mm':'ss"));
                jsonString.Append("\"");
            }
            else if (dt.Columns[col].DataType == typeof(string))
            {
                jsonString.Append("\"");
                jsonString.Append(dt.Rows[row][col].ToString().Replace("\"", "\\\""));
                jsonString.Append("\"");
            }
            else if (dt.Columns[col].DataType == typeof(bool))
            {
                jsonString.Append(Convert.ToBoolean(dt.Rows[row][col]) ? "true" : "false");
            }
            else if (dt.Columns[col].DataType == typeof(Int16) ||
                dt.Columns[col].DataType == typeof(Int32) ||
                dt.Columns[col].DataType == typeof(Int64) ||
                dt.Columns[col].DataType == typeof(Double) ||
                dt.Columns[col].DataType == typeof(Decimal) ||
                dt.Columns[col].DataType == typeof(Single) ||
                dt.Columns[col].DataType == typeof(Byte) ||
                dt.Columns[col].DataType == typeof(int) ||
                dt.Columns[col].DataType == typeof(float) ||
                dt.Columns[col].DataType == typeof(long) ||
                dt.Columns[col].DataType == typeof(short))
            {
                try
                {
                    jsonString.Append($"{dt.Rows[row][col]}");
                }
                catch
                {
                    jsonString.Append("null");
                }
            }
            else
            {
                jsonString.Append("\"");
                jsonString.Append(dt.Rows[row][col].ToString().Replace("\"", "\\\""));
                jsonString.Append("\"");
            }
            jsonString.Append(addComma);
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
