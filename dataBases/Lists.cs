using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.ObjectModel;
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

                        int tableCount = 0;
                        if (TableNames is null)
                        {
                            TableNames = new List<TableName>();
                            TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty, model.Name);
                            TableNames.Add(table);
                        }
                        else tableCount = TableNames.Count;


                        ColumnsHelpers ch = new ColumnsHelpers();

                        List<string> hasList = new List<string>();
                        ReadOnlyCollection<DbColumn> columnNames = await dr.GetColumnSchemaAsync();
                        List<Columns> columns = ch.HaveColumns(columnNames, model, $"t{tableCount}", TableNames);

                        while (dr.Read())
                        {
                            TModel dat = ch.ColumnToObject<TModel>(dr, model, TableNames, tableCount, hasList, columns);                            
                            result.Add(dat);
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
        }
        #endregion
    }
}
