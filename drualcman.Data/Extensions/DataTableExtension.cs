using drualcman.Data.Helpers;
using drualcman.Enums;
using System.Data;

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
        /// <returns></returns>
        public static List<TModel> ToList<TModel>(this DataTable dt) where TModel : new()
        {
            string[] columnNames = ColumnNamesToArray(dt);
            List<TModel> result = new List<TModel>();
            int rows = dt.Rows.Count;
            if(rows > 0)
            {
                Type model = typeof(TModel);

                List<TableName> tables = new List<TableName>();
                int tableCount = 0;
                TableName table = new TableName(model.Name, $"t{tableCount}", string.Empty, InnerDirection.NONE, string.Empty, string.Empty, model.Name);
                tables.Add(table);

                ColumnToObject co = new ColumnToObject(new ColumnsNames(columnNames, tables),
                    new InstanceModel(), tables);

                for(int index = 0; index < rows; index++)
                {
                    TModel dat = new();
                    co.SetColumnToObject(new ColumnValue(tables, dat), dt.Rows[index], dat, $"t{tableCount}");
                    result.Add(dat);
                }
            }
            return result;
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
        public static Task<List<TModel>> ToListAsync<TModel>(this DataTable dt) where TModel : new()
        {
            if(dt.Rows.Count > 0)
            {
                return Task.FromResult(dt.ToList<TModel>());
            }
            else return Task.FromResult(new List<TModel>());          //no results
        }
        #endregion
        #endregion

        #region helpers

        /// <summary>
        /// Get all column names from the table send
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string[] ColumnNamesToArray(DataTable dt)
        {
            List<string> names = new List<string>();

            foreach(DataColumn item in dt.Columns)
            {
                names.Add(item.ColumnName.ToLower());
            }

            return names.ToArray();
        }
        #endregion

    }
}
