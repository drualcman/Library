using drualcman.Data;
using drualcman.Data.Helpers;
using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
        /// Devuelve los datos de la consulta en un  List<T>
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(string sql = "", int timeout = 30) where TModel : new() =>
            ListAsync<TModel>(sql, timeout).Result;

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="cmd">Comando a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un  List<T>
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public List<TModel> List<TModel>(SqlCommand cmd, int timeout = 30) where TModel : new() =>
            ListAsync<TModel>(cmd, timeout).Result;
        #endregion

        #region async
        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un  List<T>
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public async Task<List<TModel>> ListAsync<TModel>(string sql = "", int timeout = 30) where TModel : new()
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ToList", sql, "");

            #region query
            // If a query is empty create the query from the Model

            if (string.IsNullOrWhiteSpace(sql))
            {
                SqlQueryTranslator queryTranslator = new SqlQueryTranslator(this.WhereRequired);
                sql = queryTranslator.SetQuery<TModel>();
            }
            else
            {
                TableNamesHelper tableNames = new TableNamesHelper();
                tableNames.AddTableNames<TModel>();
                CheckSqlInjection(sql, log);
            }
            #endregion

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = timeout;
            List<TModel> result = await ListAsync<TModel>(cmd, timeout);
            cmd.Dispose();
            return result;

        }

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="cmd">Comando a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un List<T>
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public Task<List<TModel>> ListAsync<TModel>(SqlCommand cmd, int timeout = 30) where TModel : new()
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ToList", "with command", cmd.CommandText);

            try
            {
                using SqlConnection cn = new SqlConnection(this.connectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = timeout;
                cmd.Connection.Open();
                using SqlDataReader dr = cmd.ExecuteReader();

                List<TModel> result = new List<TModel>();
                if (dr is not null)
                {
                    bool canRead = dr.Read();
                    if (canRead)
                    {
                        Type model = typeof(TModel);

                        int tableCount = 0;
                        TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty, model.Name);
                        List<TableName> TableNames = new List<TableName>();
                        TableNames.Add(table);

                        ReadOnlyCollection<DbColumn> columnNames = dr.GetColumnSchema();
                        //ColumnsNames ch = new ColumnsNames(columnNames, TableNames);
                        InstanceModel instanceModel = new InstanceModel();

                        ColumnSqlClientToObject columnToObject = new ColumnSqlClientToObject(new ColumnsNames(columnNames, TableNames),
                                                        instanceModel, TableNames);

                        object currentRow = new String("");
                        int t = TableNames.Count;
                        do
                        {
                            bool hasList = false;
                            TModel dat = new();
                            instanceModel.InstanceProperties(dat);
                            ColumnValue columnValue = new ColumnValue(TableNames, dat);
                            ColumnToObjectResponse response = new ColumnToObjectResponse
                            {
                                InUse = dat
                            };
                            ////first iteration
                            //response = columnToObject
                            //    .SetColumnToObject(new ColumnValue(TableNames, dat), dr, dat, "t0");
                            currentRow = dr[0];//know what is the first column asume it's the key column and no repeated
                            int i = 0;
                            do
                            {
                                response = columnToObject.SetColumnToObject(new ColumnValue(TableNames, response.InUse),
                                                    dr, response.InUse, TableNames[i].ShortName);

                                if (response.IsList)
                                {
                                    hasList = true;
                                    object listInstance = response.PropertyListInstance;
                                    string listName = response.PropertyListName;
                                    //check if have some other object like a property
                                    TableName tableList = TableNames.Where(t => t.ShortReference == response.ActualTable).FirstOrDefault();
                                    if (tableList != null)
                                    {
                                        response = columnToObject.SetColumnToObject(new ColumnValue(TableNames, response.PropertyListData),
                                                       dr, response.PropertyListData, tableList.ShortName);
                                    }

                                    listInstance.GetPropValue(listName).GetType()
                                        .GetMethod("Add").Invoke(listInstance.GetPropValue(listName),
                                                                            new[] { response.InUse });
                                    response.InUse = listInstance;
                                }

                                i++;
                                if (i >= t)
                                {
                                    i = 0;
                                    canRead = canRead = dr.Read();
                                    if (!hasList) currentRow = -1;
                                }
                            } while (canRead && currentRow.ToString() == dr[0].ToString());

                            result.Add(dat);
                            columnValue = null;
                        } while (canRead);
                    }

                }
                cmd.Connection.Close();
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                log.end(cmd.CommandText, ex.ToString() + "\n" + this.rutaDDBB);
                throw;
            }
        }
        #endregion
    }
}
