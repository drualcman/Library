using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases 
    {
        #region update
        /// <summary>
        /// Update image column from table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        /// <returns></returns>
        public bool UpdateImage(string table, string indexColumn, string index, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = UpdateImage(table, indexColumn, index, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// Update image column from table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        /// <returns></returns>
        public bool UpdateImage(string table, string indexColumn, string index, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null && image.Length > 0)
            {
                using SqlCommand cmd = new SqlCommand();
                string sql = $@"update {table} set {imageColumn} = @photo where {indexColumn} = @index;";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index", SqlDbType.VarChar);
                cmd.Parameters["@photo"].Value = image;
                cmd.Parameters["@index"].Value = index;
                result = ExecuteCommand(cmd);
                cmd.Connection.Dispose();
            }
            else result = false;
            return result;
        }
        #endregion

        #region insert
        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        public bool InsertImage(string table, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null)
            {
                using SqlCommand cmd = new SqlCommand();
                string sql = $@"insert into {table} values ({imageColumn}) values (@photo);";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index");
                cmd.Parameters["@photo"].Value = image;
                result = ExecuteCommand(cmd);
                cmd.Connection.Dispose();
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        public bool InsertImage(string table, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = InsertImage(table, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        public bool InsertImage(string table, string indexColumn, string index, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = InsertImage(table, indexColumn, index, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        public bool InsertImage(string table, string indexColumn, string index, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null)
            {
                using SqlCommand cmd = new SqlCommand();
                string sql = $@"insert into {table} values ({indexColumn}, {imageColumn}) values (@index,@photo);";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index");
                cmd.Parameters["@photo"].Value = image;
                cmd.Parameters["@index"].Value = index;
                result = ExecuteCommand(cmd);
                cmd.Connection.Dispose();
            }
            else result = false;
            return result;
        }
        #endregion

        #region Tasks
        #region update
        /// <summary>
        /// Update image column from table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        /// <returns></returns>
        public async Task<bool> UpdateImageAsync(string table, string indexColumn, string index, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = await UpdateImageAsync(table, indexColumn, index, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// Update image column from table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        /// <returns></returns>
        public async Task<bool> UpdateImageAsync(string table, string indexColumn, string index, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null && image.Length > 0)
            {
                using SqlCommand cmd = new SqlCommand();
                string sql = $@"update {table} set {imageColumn} = @photo where {indexColumn} = @index;";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index", SqlDbType.VarChar);
                cmd.Parameters["@photo"].Value = image;
                cmd.Parameters["@index"].Value = index;
                result = await ExecuteCommandAsync(cmd);
                _= cmd.Connection.DisposeAsync();
            }
            else result = false;
            return result;
        }
        #endregion

        #region insert
        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        public async Task<bool> InsertImageAsync(string table, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null)
            {
                using SqlCommand cmd = new SqlCommand();
                string sql = $@"insert into {table} values ({imageColumn}) values (@photo);";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index");
                cmd.Parameters["@photo"].Value = image;
                result = await ExecuteCommandAsync(cmd);
                _ = cmd.Connection.DisposeAsync();
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        public async Task<bool> InsertImageAsync(string table, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = await InsertImageAsync(table, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in FromBase64String</param>
        public async Task<bool> InsertImageAsync(string table, string indexColumn, string index, string imageColumn, string image)
        {
            bool result;
            if (!string.IsNullOrEmpty(image))
            {
                byte[] photo = Convert.FromBase64String(image);
                result = await InsertImageAsync(table, indexColumn, index, imageColumn, photo);
            }
            else result = false;
            return result;
        }

        /// <summary>
        /// insert image into a table
        /// </summary>
        /// <param name="table">table</param>
        /// <param name="indexColumn">index column name to get the row</param>
        /// <param name="index">index to get the row</param>
        /// <param name="imageColumn">image column name</param>
        /// <param name="image">image in bytes</param>
        public async Task<bool> InsertImageAsync(string table, string indexColumn, string index, string imageColumn, byte[] image)
        {
            bool result;
            if (image != null)
            {
                SqlCommand cmd = new SqlCommand();
                string sql = $@"insert into {table} values ({indexColumn}, {imageColumn}) values (@index,@photo);";
                cmd.CommandText = sql;
                cmd.Parameters.Add("@photo", SqlDbType.Image);
                cmd.Parameters.Add("@index");
                cmd.Parameters["@photo"].Value = image;
                cmd.Parameters["@index"].Value = index;
                result = await ExecuteCommandAsync(cmd);
                cmd.Dispose();
            }
            else result = false;
            return result;
        }
        #endregion
        #endregion
    }
}
