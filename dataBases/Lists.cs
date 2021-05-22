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
        public List<TModel> ToList<TModel>() where TModel : new() =>
            List<TModel>("", 30);


        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> ToList<TModel>(int timeout) where TModel : new() =>
            List<TModel>("", timeout);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> ToList<TModel>(string sql) where TModel : new() =>
            List<TModel>(sql, 30);

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(string querySQL, int timeout) where TModel : new()
        {
            return ListAsync<TModel>(querySQL, timeout).Result;
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
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<List<TModel>> ListAsync<TModel>(string querySQL, int timeout) where TModel : new()
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ToList", querySQL, "");
            // If a query is empty create the query from the Model
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                querySQL = SetQuery<TModel>();
            }

            try
            {
                // Comprobar que están indicando valores correctos (o casi)
                CheckSqlInjection(querySQL, log);

                using SqlConnection con = new SqlConnection(this.rutaDDBB);
                con.Open();
                using SqlCommand command = new SqlCommand(querySQL, con);
                command.CommandTimeout = timeout;
                using SqlDataReader dr = await command.ExecuteReaderAsync();

                List<TableName> tables = new List<TableName>();
                List<TModel> result = new List<TModel>();
                PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                int tableCount = 0;
                TableName newTable = new TableName(typeof(TModel).Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(newTable);

                List<string> hasList = new List<string>();
                ReadOnlyCollection<DbColumn> columnNames = null;
                bool isDirectQuery = false;
                while (await dr.ReadAsync())
                {
                    TModel item = new TModel();
                    if (columnNames is null)
                    {
                        columnNames = await dr.GetColumnSchemaAsync();
                        isDirectQuery = columnNames[0].ColumnName.IndexOf(".") < 0;
                    }

                    bool hasData = false;
                    int c = properties.Length;
                    for (int i = 0; i < c; i++)
                    {
                        DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                        string columName;
                        if (field is not null)
                        {
                            if (!field.Ignore)
                            {
                                if (isDirectQuery)
                                    columName = properties[i].Name;
                                else if (field.Inner == InnerDirection.NONE)
                                    columName = $"{tables[0].ShortName}.{properties[i].Name}";
                                else
                                {
                                    columName = string.Empty;

                                    if (Helpers.ObjectHelpers.IsGenericList(properties[i].PropertyType.FullName) &&
                                        !hasList.Contains(properties[i].PropertyType.Name))
                                    {
                                        hasList.Add(properties[i].PropertyType.Name);
                                        Type[] genericType = properties[i].PropertyType.GetGenericArguments();
                                        Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                                        properties[i].SetValue(item, Activator.CreateInstance(creatingCollectionType));
                                    }
                                    else
                                    {
                                        try
                                        {
                                            properties[i].SetValue(item,
                                                ColumnToObject(ref columnNames, dr, properties[i].PropertyType, ref tables, ref tableCount),
                                                null);
                                            hasData = true;
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
                            if (isDirectQuery)
                                columName = properties[i].Name;
                            else
                                columName = $"{tables[0].ShortName}.{properties[i].Name}";
                        }

                        if (HaveColumn(ref columnNames, columName))
                        {
                            try
                            {
                                properties[i].SetValue(item, dr[columName], null);
                                hasData = true;
                            }
                            catch { }
                        }
                    }
                    if (hasData)
                        result.Add(item);      //only add the item if have some to add
                }

                if (hasList.Any())
                {
                    //need to create a list of object who is named in the list
                    //1. create a list grouped by main model
                    //List<TModel> mainModel = result.g;

                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                log.Dispose();
                throw;
            }
        }
        #endregion

        #region helpers
        /// <summary>
        /// Know if have a column name
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="columnName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        private bool HaveColumn(ref ReadOnlyCollection<DbColumn> columns, string columnName, bool ignoreCase = true)
        {
            bool have = false;
            int t = columns.Count;
            int c = 0;
            do
            {
                if (ignoreCase)
                    have = columns[c].ColumnName.ToLower() == columnName.ToLower();
                else
                    have = columns[c].ColumnName == columnName;
                c++;
            } while (c < t && have == false);
            return have;
        }

        private object ColumnToObject(ref ReadOnlyCollection<DbColumn> columns, SqlDataReader row, Type model,
            ref List<TableName> tables, ref int tableCount)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            TableName table = tables.Where(t => t.Name == model.Name).FirstOrDefault();
            if (table is null)
            {
                tableCount++;
                table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty);
                tables.Add(table);
            }

            var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);

            int c = properties.Length;
            for (int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                string columName;
                if (field is not null)
                {
                    if (!field.Ignore)
                    {
                        if (field.Inner == InnerDirection.NONE)
                            columName = $"{table.ShortName}.{properties[i].Name}";
                        else
                        {
                            columName = string.Empty;
                            try
                            {
                                properties[i].SetValue(item,
                                    ColumnToObject(ref columns, row, properties[i].PropertyType, ref tables, ref tableCount), null);
                            }
                            catch { }
                        }
                    }
                    else
                        columName = string.Empty;
                }
                else
                {
                    columName = $"{table.ShortName}.{properties[i].Name}";
                }

                if (HaveColumn(ref columns, columName))
                {
                    try
                    {
                        properties[i].SetValue(item, row[columName], null);
                    }
                    catch { }
                }
            }
            return item;
        }
        #endregion
    }
}
