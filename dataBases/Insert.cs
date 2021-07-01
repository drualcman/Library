using System;
using System.Collections.Generic;
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
        /// Insert into a DB
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Column name in DB to be updated</param>
        /// <param name="colValue">Value to insert in DB</param>
        /// <returns></returns>
        public bool InsertInDB(string table, string colName, object colValue)
        {
            return InsertInDB(table, new string[] { colName }, new object[] { colValue });
        }

        /// <summary>
        /// Insert into a DB
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Columns name in DB to be updated</param>
        /// <param name="colValue">Values to insert in DB</param>
        /// <returns></returns>
        public bool InsertInDB(string table, string[] colName, object[] colValue)
        {
            return InsertInDB(table, colName, colValue, false) > 0;
        }

        /// <summary>
        /// Insert into a DB
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Columns name in DB to be updated</param>
        /// <param name="colValue">Values to insert in DB</param>
        /// <param name="returnScope">Return the scope_id</param>
        /// <param name="columnWithId">if table don't have auto id then give the column to return last id modified</param>
        /// <returns></returns>
        public int InsertInDB(string table, string[] colName, object[] colValue, bool returnScope, string columnWithId = "")
        {
            int result;
            if (!string.IsNullOrEmpty(table) && colName.Count() > 0 && colName.Count() == colValue.Count())
            {
                using SqlCommand cmd = SetInsert(table, colName, colValue);
                if (returnScope)
                {
                    if (string.IsNullOrEmpty(columnWithId)) cmd.CommandText += "; select SCOPE_IDENTITY()";
                    else
                    {
                        string sql = $@"DECALRE @mytable AS TABLE
                                        (
                                          Id INT
                                        )
                                    {cmd.CommandText}
                                    OUTPUT INSERTED.{columnWithId} INTO @mytable
                                    SELECT Id FROM @mytable";
                    }
                    try
                    {
                        result = Convert.ToInt32(Execute(cmd));
                    }
                    catch
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = ExecuteCommand(cmd) ? 1 : 0;
                }
                
            }
            else
            {
                result = 0;
            }
            return result;
        }

        #region tasks

        /// <summary>
        /// Insert into a DB
        /// </summary>
        /// <param name="table">Table name in DB</param>
        /// <param name="colName">Columns name in DB to be updated</param>
        /// <param name="colValue">Values to insert in DB</param>
        /// <param name="returnScope">Return the scope_id</param>
        /// <param name="columnWithId">if table don't have auto id then give the column to return last id modified</param>
        /// <returns></returns>
        public async Task<int> InsertInDBAsync(string table, string[] colName, object[] colValue, bool returnScope, string columnWithId = "")
        {
            int result;
            if (!string.IsNullOrEmpty(table) && colName.Count() > 0 && colName.Count() == colValue.Count())
            {
                using SqlCommand cmd = SetInsert(table, colName, colValue);
                await this.OpenConnectionAsync();
                cmd.Connection = this.DbConnection;
                if (returnScope)
                {
                    if (string.IsNullOrEmpty(columnWithId)) cmd.CommandText += "; select SCOPE_IDENTITY()";
                    else
                    {
                        string sql = $@"DECALRE @mytable AS TABLE
                                        (
                                          Id INT
                                        )
                                    {cmd.CommandText}
                                    OUTPUT INSERTED.{columnWithId} INTO @mytable
                                    SELECT Id FROM @mytable";
                    }
                    try
                    {
                        result = Convert.ToInt32(await ExecuteAsync(cmd));
                    }
                    catch
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = await ExecuteCommandAsync(cmd) ? 1 : 0;
                }
                
            }
            else
            {
                result = 0;
            }
            return result;
        }
        #endregion

        #endregion

        #region helpers
        private SqlCommand SetInsert(string table, string[] colName, object[] colValue)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            int i;

            SqlCommand cmd = new SqlCommand();
            //check columns
            for (i = 0; i < colName.Count(); i++)
            {
                columns.Append($"[{colName[i].Replace("[", "").Replace("]", "")}],");
                values.Append($"@value_{i},");
                cmd.Parameters.AddWithValue("@value_" + i.ToString(), colValue[i] ?? DBNull.Value);
            }
            columns.Remove(columns.Length - 1, 1);
            values.Remove(values.Length - 1, 1);
            cmd.CommandText = $"INSERT INTO {table} ({columns}) VALUES ({values});";            
            return cmd;
        }
        #endregion
    }
}
