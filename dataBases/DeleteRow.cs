using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
        #region direct queries
        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool DeleteRow(string table, string indexColumn, int index)
        {
            return DeleteRow(table, new string[] { indexColumn }, new int[] { index });
        }

        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool DeleteRow(string table, string indexColumn, string index)
        {
            return DeleteRow(table, new string[] { indexColumn }, new object[] { index });
        }

        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool DeleteRow(string table, string[] indexColumn, object[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
            {
                string sql = $@"Delete FROM {table} WHERE ";
                int i;
                //check index columns
                this.OpenConnection();
                using SqlCommand cmd = DbConnection.CreateCommand();
                for(i = 0; i < indexColumn.Count(); i++)
                {
                    sql += $"{indexColumn[i]} = @{indexColumn[i]}";
                    cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
                    if(i + 1 < indexColumn.Count()) sql += " AND ";
                }
                cmd.CommandText = sql;
                result = ExecuteCommand(cmd);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public bool DeleteRow(string table, string[] indexColumn, int[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
            {
                string sql = $@"Delete FROM {table} WHERE ";
                int i;
                //check index columns
                this.OpenConnection();
                using SqlCommand cmd = DbConnection.CreateCommand();
                for(i = 0; i < indexColumn.Count(); i++)
                {
                    sql += $"{indexColumn[i]} = @{indexColumn[i]}";
                    cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
                    if(i + 1 < indexColumn.Count()) sql += " AND ";
                }
                cmd.CommandText = sql;
                result = ExecuteCommand(cmd);

            }
            else result = false;
            return result;
        }
        #endregion

        #region Tasks
        public async Task<bool> DeleteRowAsync(string table, string indexColumn, int index) =>
            await DeleteRowAsync(table, new string[] { indexColumn }, new int[] { index });
        public async Task<bool> DeleteRowAsync(string table, string indexColumn, string index) =>
            await DeleteRowAsync(table, new string[] { indexColumn }, new object[] { index });

        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public async Task<bool> DeleteRowAsync(string table, string[] indexColumn, object[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
            {
                string sql = $@"Delete FROM {table} WHERE ";
                int i;
                //check index columns
                await this.OpenConnectionAsync();
                using SqlCommand cmd = DbConnection.CreateCommand();
                for(i = 0; i < indexColumn.Count(); i++)
                {
                    sql += $"{indexColumn[i]} = @{indexColumn[i]}";
                    cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
                    if(i + 1 < indexColumn.Count()) sql += " AND ";
                }
                cmd.CommandText = sql;
                result = await ExecuteCommandAsync(cmd);

            }
            else result = false;
            return result;
        }

        /// <summary>
        /// Delete specific column
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="indexColumn">name of Column to match on WHERE clause</param>
        /// <param name="index">Value for index</param>
        /// <returns></returns>
        public async Task<bool> DeleteRowAsync(string table, string[] indexColumn, int[] index)
        {
            bool result;
            if(!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
            {
                string sql = $@"Delete FROM {table} WHERE ";
                int i;
                //check index columns
                await this.OpenConnectionAsync();
                using SqlCommand cmd = DbConnection.CreateCommand();
                for(i = 0; i < indexColumn.Count(); i++)
                {
                    sql += $"{indexColumn[i]} = @{indexColumn[i]}";
                    cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
                    if(i + 1 < indexColumn.Count()) sql += " AND ";
                }
                cmd.CommandText = sql;
                result = await ExecuteCommandAsync(cmd);

            }
            else result = false;
            return result;
        }
        #endregion
    }
}
