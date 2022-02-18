using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases
    {
        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="sql">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string sql, int colSQL, int timeOut = 30)
        {

            log.start("GetColAync(sql, colSQL, timeOut)", sql, colSQL.ToString() + ", " + timeOut.ToString());
            string datoRetorno = string.Empty;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        this.OpenConnection();

                        try
                        {
                            using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);
                            cmd.CommandTimeout = timeOut;
                            try
                            {
                                using SqlDataReader dr = cmd.ExecuteReader();         // ejecutar el comando SQL
                                if (dr.HasRows)
                                {
                                    if (dr.Read())                                      // leer los datos
                                        datoRetorno = dr[colSQL].ToString();      // obtener el campo deseado
                                    else
                                        datoRetorno = string.Empty;
                                }
                                else
                                    datoRetorno = string.Empty;
                                dr.Close();                                     // cerrar la consulta
                            }
                            catch (Exception exReader)
                            {
                                log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);


                                throw;
                            }
                        }
                        catch (Exception exConexion)
                        {
                            log.end(null, exConexion.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }

                        if (this.LogResults)
                            log.end(datoRetorno);


                    }
                }
                else
                    datoRetorno = string.Empty;
                return datoRetorno;
            }
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="sql">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string sql, string colSQL, int timeOut = 30)
        {

            log.start("GetColAync(sql, colSQL, timeOut)", sql, colSQL.ToString() + ", " + timeOut.ToString());
            string datoRetorno = string.Empty;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        this.OpenConnection();

                        try
                        {
                            using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);
                            cmd.CommandTimeout = timeOut;
                            try
                            {
                                using SqlDataReader dr = cmd.ExecuteReader();         // ejecutar el comando SQL
                                if (dr.HasRows)
                                {
                                    if (dr.Read())                                      // leer los datos
                                        datoRetorno = dr[colSQL].ToString();      // obtener el campo deseado
                                    else
                                        datoRetorno = string.Empty;
                                }
                                else
                                    datoRetorno = string.Empty;
                                dr.Close();                                     // cerrar la consulta
                            }
                            catch (Exception exReader)
                            {
                                log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);


                                throw;
                            }
                        }
                        catch (Exception exConexion)
                        {
                            log.end(null, exConexion.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }

                        if (this.LogResults)
                            log.end(datoRetorno);


                    }
                }
                else
                    datoRetorno = string.Empty;
                return datoRetorno;
            }
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="sql">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public async Task<string> GetColAync(string sql, string colSQL, int timeOut = 30)
        {

            log.start("GetColAync(sql, colSQL, timeOut)", sql, colSQL.ToString() + ", " + timeOut.ToString());
            string datoRetorno = string.Empty;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        this.OpenConnection();

                        try
                        {
                            sql = CleanSqlDataColumns(sql);
                            using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);
                            cmd.CommandTimeout = timeOut;
                            try
                            {
                                using SqlDataReader dr = await cmd.ExecuteReaderAsync();         // ejecutar el comando SQL
                                if (dr.HasRows)
                                {
                                    if (dr.Read())                                      // leer los datos
                                        datoRetorno = dr[colSQL].ToString();      // obtener el campo deseado
                                    else
                                        datoRetorno = string.Empty;
                                }
                                else
                                    datoRetorno = string.Empty;
                                dr.Close();                                     // cerrar la consulta
                            }
                            catch (Exception exReader)
                            {
                                log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);


                                throw;
                            }
                        }
                        catch (Exception exConexion)
                        {
                            log.end(null, exConexion.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }

                        if (this.LogResults)
                            log.end(datoRetorno);

                    }
                }
                else
                    datoRetorno = string.Empty;
                return datoRetorno;
            }
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="sql">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public async Task<string> GetColAync(string sql, int colSQL, int timeOut = 30)
        {

            log.start("GetColAync(sql, colSQL, timeOut)", sql, colSQL.ToString() + ", " + timeOut.ToString());
            string datoRetorno = string.Empty;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        this.OpenConnection();

                        try
                        {
                            using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);
                            cmd.CommandTimeout = timeOut;
                            try
                            {
                                using SqlDataReader dr = await cmd.ExecuteReaderAsync();         // ejecutar el comando SQL
                                if (dr.HasRows)
                                {
                                    if (dr.Read())                                      // leer los datos
                                        datoRetorno = dr[colSQL].ToString();      // obtener el campo deseado
                                    else
                                        datoRetorno = string.Empty;
                                }
                                else
                                    datoRetorno = string.Empty;
                                dr.Close();                                     // cerrar la consulta
                            }
                            catch (Exception exReader)
                            {
                                log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);


                                throw;
                            }
                        }
                        catch (Exception exConexion)
                        {
                            log.end(null, exConexion.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }

                        if (this.LogResults)
                            log.end(datoRetorno);


                    }
                }
                else
                    datoRetorno = string.Empty;
                return datoRetorno;
            }
        }

        /// <summary>
        /// Comprobar si hay algun registro coincidente. La primera columna debe de ser un valor numerico
        /// </summary>
        /// <param name="sql">La primera columna debe de ser un valor numerico</param>
        /// <returns></returns>
        public bool ExisteEnDDBB(string sql)
        {
            return ExisteEnDDBB(sql, 30);
        }

        /// <summary>
        /// Comprobar si hay algun registro coincidente. La primera columna debe de ser un valor numerico
        /// </summary>
        /// <param name="sql">La primera columna debe de ser un valor numerico</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns></returns>
        public bool ExisteEnDDBB(string sql, int timeout)
        {

            log.start("ExisteEnDDBB(sql)", sql, "");
            bool retorno = false;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        //realizar las acciones requeridas de la consulta SQL
                        this.OpenConnection();
                        try
                        {
                            using SqlCommand command = new SqlCommand(sql, this.DbConnection);
                            command.CommandTimeout = timeout;
                            using SqlDataReader dr = command.ExecuteReader();
                            retorno = dr.HasRows;
                            dr.Close();                                     // cerrar la consulta
                        }
                        catch (Exception ex)
                        {
                            log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }
                    }
                }
                else
                    retorno = false;
            }
            if (this.LogResults)
                log.end(retorno);


            return retorno;
        }

        /// <summary>
        /// Comprobar si hay algun registro coincidente. La primera columna debe de ser un valor numerico
        /// </summary>
        /// <param name="sql">La primera columna debe de ser un valor numerico</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns></returns>
        public async Task<bool> HasRowsAsync(string sql, int timeout = 30)
        {

            log.start("ExisteEnDDBB(sql)", sql, "");
            bool retorno = false;
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        //realizar las acciones requeridas de la consulta SQL
                        this.OpenConnection();
                        try
                        {
                            using SqlCommand command = new SqlCommand(sql, this.DbConnection);
                            command.CommandTimeout = timeout;
                            using SqlDataReader dr = await command.ExecuteReaderAsync();
                            retorno = dr.HasRows;
                            dr.Close();                                     // cerrar la consulta
                        }
                        catch (Exception ex)
                        {
                            log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                            throw;
                        }
                    }
                }
                else
                    retorno = false;
            }
            if (this.LogResults)
                log.end(retorno);


            return retorno;
        }

        /// <summary>
        /// Devuelve datos de la consulta en formato XML
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="timeout">time out in seconds</param>
        /// <returns>Devuelve los datos en formato XML, Si hay error devuelve el texto del error</returns>
        public string ConsultarConXML(string sql, int timeout = 30)
        {

            log.start("ConsultarConXML", sql, "");
            // Que no sea una cadena vacía
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula.\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula.");
            }
            if (checkQuery(sql))
            {
                //
                // Comprobar que están indicando valores correctos (o casi)
                //

                bool ok = true;
                // no permitir comentarios ni algunas instrucciones maliciosas
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (sql.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("EXEC") < 0) // Comprobar que realmente se use SELECT, o EXEC
                {
                    ok = false;
                }
                else
                    ok = true;

                // Comprobar que realmente se use SELECT, o EXEC
                if (sql.ToUpper().IndexOf("SELECT") < 0 && ok == false)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables.SQL: " + sql + "\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables. SQL: " + sql);
                }
                else
                {
                    this.OpenConnection();
                    using SqlDataAdapter da = new SqlDataAdapter(sql, this.DbConnection);
                    da.SelectCommand.CommandTimeout = timeout;
                    using DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);


                        throw;
                    }
                    if (this.LogResults)
                        log.end(ds);


                    return ds.GetXml();
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);


                return "<NewDataSet />";
            }
        }

        /// <summary>
        /// Obtiene el siguiente Id de la tabla
        /// </summary>
        /// <param name="Tabla">Nombre de la tabla</param>
        /// <returns>Devuelve el siguiente entero disponible</returns>
        public int ObtenerNuevoId(string Tabla)
        {
            string sql = "SELECT IDENT_CURRENT('" + Tabla + "')";

            log.start("ObtenerNuevoId(tabla)", sql, Tabla);
            if (checkQuery(sql))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (sql.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else
                {
                    //cargar las listas del blog con los datos de la base de datos MySQL                
                    int newId = 0;

                    try
                    {
                        string dato = ObtenerDato(sql, 0);
                        //if (string.IsNullOrEmpty(dato)) dato = "0";
                        newId = Convert.ToInt32(dato);
                        newId++;
                    }
                    catch (Exception ex)
                    {
                        log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                        throw;
                    }
                    if (this.LogResults)
                        log.end(newId);


                    return newId;
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);


                return -1;      //devuelve un valor que al intentar usar como ID no se puede ya que ha habido un intento que ataque por injeccion de codigo
            }
        }

        /// <summary>
        /// Obtiene el siguiente Id de la tabla
        /// </summary>
        /// <param name="Tabla">Nombre de la tabla</param>
        /// <param name="col">Nombre del campo que contiene el Id</param>
        /// <returns>Devuelve el siguiente entero disponible</returns>
        public int ObtenerNuevoId(string Tabla, string col)
        {
            string sql = "SELECT TOP 1 " + col + " FROM " + Tabla +
                " ORDER BY " + col + " DESC";

            log.start("ObtenerNuevoId(tabla, col)", sql, Tabla + "," + col);
            if (checkQuery(sql))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (sql.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else
                {
                    int newId = 0;
                    try
                    {
                        string dato = ObtenerDato(sql, 0);
                        if (string.IsNullOrEmpty(dato)) newId = 0;
                        else newId = Convert.ToInt32(dato);
                        newId++;
                    }
                    catch (Exception ex)
                    {
                        log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                        throw;
                    }

                    if (this.LogResults)
                        log.end(newId);


                    return newId;
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);


                return -1;      //devuelve un valor que al intentar usar como ID no se puede ya que ha habido un intento que ataque por injeccion de codigo
            }
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string sql)
        {

            log.start("ConsutarConDataSet", sql, "");
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula.");
            }

            if (checkQuery(sql))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (sql.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                }
                else
                {
                    if ((sql.ToUpper().IndexOf("EXEC ") < 0))
                        sql = "EXEC " + sql;

                    this.OpenConnection();
                    using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);

                    string datoRetorno = string.Empty;
                    try
                    {
                        using SqlDataReader dr = cmd.ExecuteReader();         // ejecutar el comando SQL
                        if (dr.Read())                                      // leer los datos
                            datoRetorno = dr.GetValue(0).ToString();        // obtener el campo deseado               
                        else
                            datoRetorno = string.Empty;
                    }
                    catch (Exception exReader)
                    {
                        log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);
                        throw;
                    }
                    if (this.LogResults)
                        log.end(datoRetorno);

                    return datoRetorno;
                }
            }
            else
                return string.Empty;

        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string sql, string param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return escalarSP(sql + " " + param);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string sql, string[] param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return escalarSP(sql + " " + doParam);
        }

        /// <summary>
        /// Obtener los datos de una consulta sql en formato ArrayList
        /// </summary>
        /// <param name="sql">Consulta a ejecutar</param>
        /// <returns></returns>
        public ArrayList getData(string sql)
        {
            ArrayList retorno = new ArrayList();

            log.start("getData(querySQ)", sql, "");
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                try
                {
                    using DataSet ds = ConsultarConDataSet(sql);

                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow linea in table.Rows)
                        {
                            if (table.Rows.Count > 1)
                            {
                                ArrayList data = new ArrayList();
                                foreach (var columna in linea.ItemArray)
                                {
                                    data.Add(columna);
                                }
                                retorno.Add(data);
                            }
                            else
                            {
                                retorno.Add(linea);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                    throw;
                }
            }
            if (this.LogResults)
                log.end(retorno);


            return retorno;
        }

        /// <summary>
        /// Obtener los datos de una consulta sql en formato ArrayList
        /// </summary>
        /// <param name="sql">Consulta a ejecutar</param>
        /// <returns></returns>
        public ArrayList getData(string sql, int cols)
        {

            log.start("GetData(sql, cols)", sql, cols.ToString());
            ArrayList retorno = new ArrayList();
            if (string.IsNullOrWhiteSpace(sql))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);


                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(sql))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (sql.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);


                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else if (sql.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);


                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + sql);
                    }
                    else
                    {
                        try
                        {
                            this.OpenConnection();
                            using SqlCommand cmd = new SqlCommand(sql, this.DbConnection);
                            using SqlDataReader ds = cmd.ExecuteReader();

                            //localizar los datos en la base de datos
                            try
                            {
                                if (ds.Read())
                                {
                                    for (int i = 0; i < cols; i++)
                                    {
                                        try
                                        {
                                            retorno.Add(ds.GetValue(i));
                                        }
                                        catch (Exception ex)
                                        {
                                            retorno.Add("GetValue(" + i.ToString() + "): " + ex.ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception exRead)
                            {
                                retorno.Add("Error in READ: " + exRead.ToString());
                            }


                        }
                        catch (Exception ex)
                        {
                            log.end(sql, ex.ToString() + "\n" + this.rutaDDBB);


                            throw;

                        }

                        if (this.LogResults)
                            log.end(retorno);


                        return retorno;
                    }
                }
                else
                {
                    log.end(null, "No ha superado la prueba de validacion\n" + this.rutaDDBB);


                    throw new ArgumentException("La cadena no puede ser nula");
                }
            }
        }

    }
}