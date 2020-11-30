using System;
using System.Data.SqlClient;
using System.Linq;

namespace drualcman
{
	public partial class dataBases
	{
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
			if (!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
			{
				string sql = $@"Delete FROM {table} WHERE ";
				int i;
				//check index columns
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = this.cnnDDBB();
				for (i = 0; i < indexColumn.Count(); i++)
				{
					sql += $"{indexColumn[i]} = @{indexColumn[i]}";
					cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
					if (i + 1 < indexColumn.Count()) sql += " AND ";
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
			if (!string.IsNullOrEmpty(table) && indexColumn.Count() > 0 && index.Count() > 0)
			{
				string sql = $@"Delete FROM {table} WHERE ";
				int i;
				//check index columns
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = this.cnnDDBB();
				for (i = 0; i < indexColumn.Count(); i++)
				{
					sql += $"{indexColumn[i]} = @{indexColumn[i]}";
					cmd.Parameters.AddWithValue($"@{indexColumn[i]}", index[i]);
					if (i + 1 < indexColumn.Count()) sql += " AND ";
				}
				cmd.CommandText = sql;
				result = ExecuteCommand(cmd);
			}
			else result = false;
			return result;
		}
	}
}
