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
                    TableName newTable = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                    tables.Add(newTable);

                    List<string> hasList = new List<string>();
                    ReadOnlyCollection<DbColumn> columnNames = await dr.GetColumnSchemaAsync();
                    bool isDirectQuery = columnNames[0].ColumnName.IndexOf(".") < 0;
                    List<Columns> columns = HaveColumns(columnNames, model);

                    int c = columns.Count;
                    while (await dr.ReadAsync())
                    {
                        result.Add((TModel)ColumnToObject(ref columnNames, dr, model, ref tables, ref tableCount, ref hasList, isDirectQuery));
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
        private List<Columns> HaveColumns(ReadOnlyCollection<DbColumn> columns, Type model, bool ignoreCase = true)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            int t = columns.Count;
            int p = properties.Length;

            for (int i = 0; i < p; i++)
            {
                int c = 0;
                bool have;
                do
                {
                    if (ignoreCase)
                        have = columns[c].ColumnName.ToLower() == properties[i].Name.ToLower();
                    else
                        have = columns[c].ColumnName == properties[i].Name;
                    c++;
                } while (c < t && have == false);

                if (have)
                {
                    result.Add(new Columns { Column = properties[i], Options = properties[i].GetCustomAttribute<DatabaseAttribute>() });
                }
            }            
            return result;
        }

        private object ColumnToObject(ref ReadOnlyCollection<DbColumn> columnNames, SqlDataReader row, Type model,
            ref List<TableName> tables, ref int tableCount, ref List<string> hasList, bool isDirectQuery)
        {
            List<Columns> columns = HaveColumns(columnNames, model);

            TableName table = tables.Where(t => t.Name == model.Name).FirstOrDefault();
            if (table is null)
            {
                tableCount++;
                table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(table);
            }

            var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);

            int c = columns.Count;
            for (int i = 0; i < c; i++)
            {
                string columName;
                if (columns[i].Options is not null)
                {
                    if (!columns[i].Options.Ignore)
                    {
                        if (isDirectQuery)
                            columName = columns[i].Column.Name;
                        else if (columns[i].Options.Inner == InnerDirection.NONE)
                            columName = $"{tables[0].ShortName}.{columns[i].Column.Name}";
                        else
                        {
                            columName = string.Empty;
                            if (Helpers.ObjectHelpers.IsGenericList(columns[i].Column.PropertyType.FullName) &&
                                            !hasList.Contains(columns[i].Column.PropertyType.Name))
                            {
                                hasList.Add(columns[i].Column.PropertyType.Name);
                                Type[] genericType = columns[i].Column.PropertyType.GetGenericArguments();
                                Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                                columns[i].Column.SetValue(item, Activator.CreateInstance(creatingCollectionType));
                            }
                            else
                            {
                                try
                                {
                                    columns[i].Column.SetValue(item,
                                        ColumnToObject(ref columnNames, row, columns[i].Column.PropertyType, ref tables, ref tableCount, ref hasList, isDirectQuery),
                                        null);
                                }
                                catch { }
                            }
                        }
                    }
                    else
                        columName = string.Empty;
                }
                else
                {
                    columName = $"{table.ShortName}.{columns[i].Column.Name}";
                }

                try
                {
                    if (columns[i].Column.PropertyType.Name == typeof(bool).Name)
                        columns[i].Column.SetValue(item, Convert.ToBoolean(row[columName]), null);
                    else
                        columns[i].Column.SetValue(item, row[columName], null);
                }
                catch { }
            }
            return item;
        }
        #endregion
    }
}
