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
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>() where TModel : new() =>
            List<TModel>("", 30);


        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(int timeout) where TModel : new() =>
            List<TModel>("", timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(string sql) where TModel : new() =>
            List<TModel>(sql, 30);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(string sql, int timeout) where TModel : new()
        {
            return ListAsync<TModel>(sql, timeout).Result;
        }
        #endregion

        #region async
        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public async Task<List<TModel>> ListAsync<TModel>() where TModel : new() =>
            await ListAsync<TModel>(SetQuery<TModel>());

        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<List<TModel>> ListAsync<TModel>(int timeout) where TModel : new() =>
            await ListAsync<TModel>(SetQuery<TModel>(), timeout);

        /// <summary>
        /// Executer query and return List of model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<TModel>> ListAsync<TModel>(string query) where TModel : new() =>
            await ListAsync<TModel>(query, 30);


        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<List<TModel>> ListAsync<TModel>(string sql, int timeout) where TModel : new()
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
                if (dr.HasRows)
                {
                    Type model = typeof(TModel);

                    List<TableName> tables = new List<TableName>();
                    int tableCount = 0;
                    TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                    tables.Add(table);

                    List<string> hasList = new List<string>();
                    ReadOnlyCollection<DbColumn> columnNames = await dr.GetColumnSchemaAsync();
                    List<Columns> columns = HaveColumns(columnNames, model, table.ShortName, true);
                    while (await dr.ReadAsync())
                    {
                        result.Add((TModel)ColumnToObject(ref columnNames, dr, model, ref tables, ref tableCount, ref hasList, ref columns));
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
            catch (Exception ex)
            {
                log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);
                throw;
            }
            finally
            {
                log.Dispose();
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
        private List<Columns> HaveColumns(ReadOnlyCollection<DbColumn> columns, Type model, string shortName, bool ignoreCase)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            int t = columns.Count;
            int p = properties.Length;
            bool isDirectQuery = columns[0].ColumnName.IndexOf(".") < 0;
            for (int r = 0; r < t; r++)
            {
                int c = -1;
                bool have = false;                
                string columnCompare = string.Empty;
                DatabaseAttribute options = null;
                while (c < p && have == false)
                {
                    c++;
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
                            columnCompare = columns[r].ColumnName.Replace($"{shortName}.", "");
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
                }
                if (have)
                {
                    result.Add(new Columns { Column = properties[c], Options = options, TableName = shortName, ColumnName = columns[r].ColumnName });
                }
            }
            return result;
        }

        private object ColumnToObject(ref ReadOnlyCollection<DbColumn> columnNames, SqlDataReader row, Type model,
            ref List<TableName> tables, ref int tableCount, ref List<string> hasList, ref List<Columns> columns)
        {

            TableName table = tables.Where(t => t.Name == model.Name).FirstOrDefault();
            if (table is null)
            {
                tableCount++;
                table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(table);
            }

            if (columns.Where(t => t.TableName == table.ShortName).FirstOrDefault() == null)
            {
                columns = HaveColumns(columnNames, model, table.ShortName, true);
            }
            var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);

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
                        }
                        else
                        {
                            try
                            {
                                if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                                    columns[i].Column.SetValue(item, Convert.ToBoolean(row[columns[i].ColumnName]), null);
                                else
                                    columns[i].Column.SetValue(item, row[columns[i].ColumnName], null);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                                columns[i].Column.SetValue(item, Convert.ToBoolean(row[columns[i].ColumnName]), null);
                            else
                                columns[i].Column.SetValue(item, row[columns[i].ColumnName], null);
                        }
                        catch { }
                    }
                }
            }
            return item;
        }
        #endregion
    }
}
