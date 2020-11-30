using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace drualcman
{
    public partial class dataBases
    {
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
        /// <param name="returnScope">Return a column name</param>
        /// <returns></returns>
        public int InsertInDB(string table, string[] colName, object[] colValue, bool returnScope)
        {
            int result;
            if (!string.IsNullOrEmpty(table) && colName.Count() > 0 && colName.Count() == colValue.Count())
            {
                string columns = string.Empty;
                string values = string.Empty;
                string logValues = string.Empty;
                int i;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = this.cnnDDBB();
                //check columns
                for (i = 0; i < colName.Count(); i++)
                {
                    columns += colName[i] + ",";
                    values += "@value_" + i.ToString() + ",";
                    if (colValue[i] != null) logValues += colValue[i].ToString() + ",";
                    else logValues += "NULL,";
                    cmd.Parameters.AddWithValue("@value_" + i.ToString(), colValue[i]);
                }
                columns = columns.Remove(columns.Length - 1, 1);
                values = values.Remove(values.Length - 1, 1);
                string sql = "INSERT INTO " + table + "(" + columns + ") VALUES (" + values + ");";
                cmd.CommandText = sql;
                if (returnScope)
                {
                    cmd.CommandText += "; select SCOPE_IDENTITY()";
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
                cmd.Dispose();
            }
            else
            {
                result = 0;
            }
            return result;
        }

    }
}
