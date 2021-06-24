using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using drualcman.Data.Extensions;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.ObjectModel;
using drualcman.Attributes;
using drualcman.Enums;
using drualcman.Data;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(string sql = "", int timeout = 30) where TModel : new()
        {
            return ListAsync<TModel>(sql, timeout).Result;
        }
        #endregion

        #region async

        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<List<TModel>> ListAsync<TModel>(int timeout = 30) where TModel : new() =>
            await ListAsync<TModel>(SetQuery<TModel>(), timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<List<TModel>> ListAsync<TModel>(string sql, int timeout = 30) where TModel : new()
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ToList", sql, "");

            #region query
            // If a query is empty create the query from the Model
            if (string.IsNullOrWhiteSpace(sql))
            {
                sql = SetQuery<TModel>();
            }
            else 
            {
                // check the sql if have some injection throw a exception
                CheckSqlInjection(sql, log);
            }
            #endregion

            try
            {
                using SqlDataReader dr = await this.ReaderAsync(sql, timeout);

                List<TModel> result = new List<TModel>();
                if (dr is not null)
                {
                    if (dr.HasRows)
                    {
                        Type model = typeof(TModel);

                        List<TableName> tables = new List<TableName>();
                        int tableCount = 0;
                        TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                        tables.Add(table);

                        List<string> hasList = new List<string>();
                        ReadOnlyCollection<DbColumn> columnNames = await dr.GetColumnSchemaAsync();
                        int c = 0;
                        List<Columns> columns = HaveColumns(columnNames, model, table.ShortName, true, 0, ref tables, ref tableCount, out c);
                        while (await dr.ReadAsync())
                        {
                            result.Add((TModel)ColumnToObject(ref columnNames, dr, model, ref hasList, columns, ref tables));
                        }

                        //if (hasList.Any())
                        //{
                        //    //need to create a list of object who is named in the list
                        //    //1. create a list grouped by main model
                        //    //List<TModel> mainModel = result.g;

                        //}
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);
                throw;
            }
            finally
            {
                
            }
        }
        #endregion

        #region helpers

        /// <summary>
        /// Get all the columns properties need from the query used
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="model"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private List<Columns> HaveColumns(ReadOnlyCollection<DbColumn> columns, Type model, string shortName, 
            bool ignoreCase, int row, ref List<TableName> tables, ref int tableCount, out int counter)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            int t = columns.Count;
            int p = properties.Length;
            bool isDirectQuery = columns[0].ColumnName.IndexOf(".") < 0;
            string columnCompare;
            counter = 0;
            for (int r = row; r < t; r++)
            {
                int c = 0;
                bool have = false;
                bool escape = false;
                DatabaseAttribute options = null;
                while (c < p && have == false && escape == false)
                {
                    options = properties[c].GetCustomAttribute<DatabaseAttribute>();
                    if (isDirectQuery)
                    {
                        columnCompare = columns[r].ColumnName;
                    }
                    else
                    {
                        if (options is null)
                        {
                            columnCompare = columns[r].ColumnName.Replace($"{shortName}.", "");
                        }
                        else if (options.Inner == InnerDirection.NONE)
                        {
                            if (options.Name == columns[r].ColumnName.Replace($"{shortName}.", "")) columnCompare = properties[c].Name;
                            else columnCompare = columns[r].ColumnName.Replace($"{shortName}.", "");
                        }
                        else
                        {
                            TableName table = tables.Where(t => t.Name == properties[c].Name).FirstOrDefault();
                            if (table is null)
                            {
                                tableCount++;
                                shortName = $"t{tableCount}";
                                table = new TableName(properties[c].Name, shortName, $"t{tableCount-1}", options.Inner, options.InnerColumn, options.InnerIndex, properties[c]);
                                tables.Add(table);                                
                                result.AddRange(HaveColumns(columns, properties[c].PropertyType, shortName, ignoreCase, r, ref tables, ref tableCount, out r));                                
                                escape = true;
                                have = false;
                            }
                            else shortName = table.ShortName;
                            columnCompare = string.Empty;
                        }
                    }
                    if (!escape)
                    {
                        if (ignoreCase)
                            have = columnCompare.ToLower() == properties[c].Name.ToLower();
                        else
                            have = columnCompare == properties[c].Name;
                    }
                    c++;
                }
                if (have)
                {
                    c--;
                    string propertyType;
                    if (properties[c].PropertyType.Name == typeof(bool).Name) propertyType = "bool";
                    else if (properties[c].PropertyType.Name == typeof(int).Name) propertyType = "int";
                    else if (properties[c].PropertyType.Name == typeof(long).Name) propertyType = "long";
                    else if (properties[c].PropertyType.Name == typeof(double).Name) propertyType = "double";
                    else if (properties[c].PropertyType.Name == typeof(decimal).Name) propertyType = "decimal";
                    else if (properties[c].PropertyType.Name == typeof(float).Name) propertyType = "float";
                    else if (properties[c].PropertyType.Name == typeof(short).Name) propertyType = "short";
                    else if (properties[c].PropertyType.Name == typeof(byte).Name) propertyType = "byte";
                    else if (properties[c].PropertyType.Name == typeof(DateTime).Name) propertyType = "date";
                    else propertyType = "text";

                    result.Add(new Columns { Column = properties[c], Options = options, TableShortName = shortName, ColumnName = columns[r].ColumnName, PropertyType = propertyType, TableIndex = tableCount });
                }
            }
            return result;
        }

        private object ColumnToObject(ref ReadOnlyCollection<DbColumn> columnNames, 
            SqlDataReader row, Type model,ref List<string> hasList, 
            List<Columns> columns, ref List<TableName> tables)
        {
            var item = Activator.CreateInstance(model);// Assembly.GetAssembly(model).CreateInstance(model.FullName, true);

            int t = tables.Count;
            int i;
            for (i = 1; i < t; i++)
            {
                tables[i].Instance.SetValue(item, Activator.CreateInstance(tables[i].Instance.PropertyType), null);
            }

            int c = columns.Count;
            for (i = 0; i < c; i++)
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
                                        ColumnToObject(ref columnNames, row, columns[i].Column.PropertyType, ref hasList, columns, ref tables),
                                        null);
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    if (columns[i].TableIndex > 0)
                                    {
                                        SetValue(columns[i].PropertyType, columns[i].Column, item.GetPropValue(tables[columns[i].TableIndex].Name), row[columns[i].ColumnName]);
                                    }
                                    else
                                    {
                                        SetValue(columns[i].PropertyType, columns[i].Column, item, row[columns[i].ColumnName]);
                                    }
                                }
                                catch (Exception ex) { string err = ex.Message; }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (columns[i].TableIndex > 0)
                            {
                                SetValue(columns[i].PropertyType, columns[i].Column, item.GetPropValue(tables[columns[i].TableIndex].Name), row[columns[i].ColumnName]);
                            }
                            else
                            {
                                SetValue(columns[i].PropertyType, columns[i].Column, item, row[columns[i].ColumnName]);
                            }
                        }
                        catch (Exception ex){ string err = ex.Message; }
                    }
                }
            }
            return item;
        }

        private void SetValue(string propertyType, PropertyInfo sender, object destination, object value)
        {
            switch (propertyType)
            {
                case "bool":
                    if (int.TryParse(value.ToString(), out int test))
                    {
                        if (test == 0) sender.SetValue(destination, false);
                        else sender.SetValue(destination, true);
                    }
                    else sender.SetValue(destination, Convert.ToBoolean(value));
                    break;
                case "int":
                    sender.SetValue(destination, Convert.ToInt32(value));
                    break;
                case "double":
                    sender.SetValue(destination, Convert.ToDouble(value));
                    break;
                case "float":
                    sender.SetValue(destination, Convert.ToSingle(value));
                    break;
                case "decimal":
                    sender.SetValue(destination, Convert.ToDecimal(value));
                    break;
                case "long":
                    sender.SetValue(destination, Convert.ToInt64(value));
                    break;
                case "short":
                    sender.SetValue(destination, Convert.ToInt16(value));
                    break;
                case "byte":
                    sender.SetValue(destination, Convert.ToByte(value));
                    break;
                //case "number":
                //    sender.SetValue(destination, value);
                //    break;
                case "date":
                    sender.SetValue(destination, Convert.ToDateTime(value));
                    break;
                default:
                    sender.SetValue(destination, value.ToString());
                    break;
            }
        }
        #endregion
    }
}
