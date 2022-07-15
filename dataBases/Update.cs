using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        /// <summary>
        /// Update specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Column name in DB to be updated</param>
        /// <param name="colValue">Value to insert in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool UpdateColumn(string table, string colName, object colValue, string indexColumn, int index)
        {
            return UpdateColumn(table, new string[] { colName }, new object[] { colValue }, new string[] { indexColumn }, new object[] { index });
        }

        /// <summary>
        /// Update specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Column name in DB to be updated</param>
        /// <param name="colValue">Value to insert in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool UpdateColumn(string table, string colName, object colValue, string indexColumn, object index)
        {
            return UpdateColumn(table, new string[] { colName }, new object[] { colValue }, new string[] { indexColumn }, new object[] { index });
        }

        /// <summary>
        /// Update specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Columns name in DB to be updated</param>
        /// <param name="colValue">Values to insert in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool UpdateColumn(string table, string[] colName, object[] colValue, string indexColumn, int index)
        {
            return UpdateColumn(table, colName, colValue, new string[] { indexColumn }, new object[] { index });
        }

        /// <summary>
        /// Update specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Columns name in DB to be updated</param>
        /// <param name="colValue">Values to insert in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool UpdateColumn(string table, string[] colName, object[] colValue, string indexColumn, object index)
        {
            return UpdateColumn(table, colName, colValue, new string[] { indexColumn }, new object[] { index });
        }

        /// <summary>
        /// Update specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Column name in DB to be updated</param>
        /// <param name="colValue">Value to insert in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool UpdateColumn(string table, string[] colName, object[] colValue, string[] indexColumn, object[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && colName.Count() > 0 && colName.Count() == colValue.Count() && indexColumn.Count() > 0)
            {
                using SqlCommand cmd = SetUpdate(table, colName, colValue, indexColumn, index);
                result = ExecuteCommand(cmd);

            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region tasks
        public async Task<bool> UpdateColumnAsync(string table, string colName, object colValue, string indexColumn, int index) =>
            await UpdateColumnAsync(table, new string[] { colName }, new object[] { colValue }, new string[] { indexColumn }, new object[] { index });
        public async Task<bool> UpdateColumnAsync(string table, string colName, object colValue, string indexColumn, object index) =>
            await UpdateColumnAsync(table, new string[] { colName }, new object[] { colValue }, new string[] { indexColumn }, new object[] { index });
        public async Task<bool> UpdateColumnAsync(string table, string[] colName, object[] colValue, string indexColumn, int index) =>
            await UpdateColumnAsync(table, colName, colValue, new string[] { indexColumn }, new object[] { index });
        public async Task<bool> UpdateColumnAsync(string table, string[] colName, object[] colValue, string indexColumn, object index) =>
            await UpdateColumnAsync(table, colName, colValue, new string[] { indexColumn }, new object[] { index });
        public async Task<bool> UpdateColumnAsync(string table, string[] colName, object[] colValue, string[] indexColumn, object[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && colName.Count() > 0 && colName.Count() == colValue.Count() && indexColumn.Count() > 0)
            {
                using SqlCommand cmd = SetUpdate(table, colName, colValue, indexColumn, index);
                result = await ExecuteCommandAsync(cmd);

            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region helpers
        private SqlCommand SetUpdate(string table, string[] colName, object[] colValue, string[] indexColumn, object[] index)
        {
            int i;

            using SqlCommand cmd = new SqlCommand();
            StringBuilder sql = new StringBuilder($"UPDATE {table} SET ");
            //check columns
            for(i = 0; i < colName.Count(); i++)
            {
                if (colValue[i] is not null)
                {
                    if (colValue[i].GetType() == typeof(DateTime))
                    {
                        cmd.Parameters.AddWithValue("@value_" + i.ToString(), Convert.ToDateTime(colValue[i]).ToUniversalTime());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@value_" + i.ToString(), colValue[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@value_" + i.ToString(), DBNull.Value);
                }
                sql.Append($" [{colName[i].Replace("[", "").Replace("]", "")}] = @value_{i},");
            }
            sql.Remove(sql.Length - 1, 1);
            if(indexColumn.Count() > 0)
            {
                sql.Append("  WHERE ");
                for(i = 0; i < indexColumn.Count(); i++)
                {
                    cmd.Parameters.AddWithValue($"@index_{i}", index[i]);
                    sql.Append($" [{indexColumn[i].Replace("[", "").Replace("]", "")}]  = @index_{i} AND ");
                }
                sql.Remove(sql.Length - 4, 4);
            }
            sql.Append(";");
            cmd.CommandText = sql.ToString();
            return cmd;
        }
        #endregion
    }
}
