using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases 
    {
        /// <summary>
        /// Prevenir INJECTION SQL en las consultas
        /// </summary>
        /// <param name="query">consulta enviada</param>
        /// <returns></returns>
        private bool checkQuery(string query)
        {
            bool resultado = false;
            if (this.dbControl == true)
            {
                if (!string.IsNullOrEmpty(query))
                {
                    if (query.ToUpper().IndexOf("INFORMATION_SCHEMA") >= 0)
                        resultado = false;
                    else if (query.ToLower().IndexOf("sysobjects") >= 0)
                        resultado = false;
                    else if (query.ToLower().IndexOf("syscolumns") >= 0)
                        resultado = false;
                    else if (query.ToLower().IndexOf(this.rutaDDBB) >= 0)
                        resultado = false;
                    else if (query.ToUpper().IndexOf("BENCHMARK(") >= 0)
                        resultado = false;
                    else if (this.ChrControl == true)
                    {
                        int tiene = query.ToLower().IndexOf("chr(");
                        if (tiene >= 0)
                            resultado = false;
                        else
                            resultado = true;
                    }
                    else
                        resultado = true;
                }
                else
                    resultado = false;
            }
            else resultado = true;
            return resultado;
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="querySQL">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string querySQL, int colSQL)
        {
            return ObtenerDato(querySQL, colSQL, 30);
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="querySQL">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string querySQL, int colSQL, int timeOut)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ObtenerDato(querySQL, colSQL, timeOut)", querySQL, colSQL.ToString() + ", " + timeOut.ToString());
            string datoRetorno = string.Empty;
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(querySQL))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (querySQL.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(this.rutaDDBB))
                        {
                            try
                            {
                                SqlCommand cmd = new SqlCommand(querySQL, con);
                                cmd.CommandTimeout = timeOut;
                                con.Open();

                                try
                                {
                                    SqlDataReader dr = cmd.ExecuteReader();         // ejecutar el comando SQL
                                    if (dr.Read())                                      // leer los datos
                                        datoRetorno = dr.GetValue(colSQL).ToString();      // obtener el campo deseado
                                    else
                                        datoRetorno = string.Empty;
                                    dr.Close();                                     // cerrar la consulta
                                    dr.Dispose();                                   // cerrar la conexión
                                }
                                catch (Exception exReader)
                                {
                                    cmd.Dispose();              // cerrar la conexión
                                    con.Close();
                                    log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);
                                    log.Dispose();

                                    throw exReader;
                                }
                                cmd.Dispose();              // cerrar la conexión
                            }
                            catch (Exception exConexion)
                            {
                                con.Close();
                                con.Dispose();
                                log.end(null, exConexion.ToString() + "\n" + this.rutaDDBB);
                                log.Dispose();

                                throw exConexion;
                            }
                            // cerrar la conexión
                            con.Close();
                            con.Dispose();
                        }
                        if (this.LogError) log.end(datoRetorno, this.rutaDDBB);
                        log.Dispose();

                        return datoRetorno;
                    }
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="querySQL">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string querySQL, string colSQL)
        {
            return ObtenerDato(querySQL, colSQL, 30);
        }

        /// <summary>
        /// Obtiene un dato en concreo de la base de datos
        /// </summary>
        /// <param name="querySQL">Consulta a SQL ejecutar</param> 
        /// <param name="colSQL">Columna para situarnos</param> 
        /// <param name="timeOut">Execution timeout</param> 
        /// <returns>
        /// Devuelve una cadena con el resultado
        /// Si hay error devuelve el mensasje de error
        /// </returns>
        public string ObtenerDato(string querySQL, string colSQL, int timeOut)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ObtenerDato(querySQL, colSQL)", querySQL, colSQL + ", " + timeOut.ToString());
            string datoRetorno;
            try
            {
                DataTable dt = ConsultarConDataTable(querySQL);
                datoRetorno = dt.Rows[0][colSQL].ToString();
            }
            catch (Exception ex)
            {
                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                log.Dispose();

                throw ex;
            }
            if (this.LogError) log.end(datoRetorno, this.rutaDDBB);
            log.Dispose();

            return datoRetorno;
        }

        /// <summary>
        /// Ejecuta un comando SQL
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Retorna el valor obtenido del web service al ejecutar el comando
        /// </returns>
        public bool ComandoSQL(string querySQL)
        {
            return EjecutarSQL(querySQL);
        }

        /// <summary>
        /// Ejecuta un comando SQL
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Retorna el valor obtenido del web service al ejecutar el comando
        /// </returns>
        public bool EjecutarSQL(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("EjecutarSQL(querySQL)", querySQL, "");
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(querySQL))
                {
                    //
                    // Comprobar que están indicando valores correctos (o casi)
                    //
                    // Que no sea una cadena vacía                    

                    if ((querySQL.ToUpper().IndexOf("UPDATE ") < 0))
                    {
                        if ((querySQL.ToUpper().IndexOf("INSERT ") < 0))
                        {
                            if ((querySQL.ToUpper().IndexOf("DELETE ") < 0))
                            {
                                if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                                {
                                    if ((querySQL.ToUpper().IndexOf("DROP ") < 0))
                                    {
                                        if ((querySQL.ToUpper().IndexOf("ALTER ") < 0))
                                        {
                                            if ((querySQL.ToUpper().IndexOf("CREATE ") < 0))
                                            {
                                                string err = "La cadena debe ser: " + "\r\n" +
                                                    "UPDATE < tabla > SET < campo=valor >" + "\r\n" +
                                                    "INSERT INTO < tabla > VALUES < campo=valor >" + "\r\n" +
                                                    "DELETE < tabla > WHERE < condicion >" + "\r\n" +
                                                    "EXEC  < Storage Proccess > < Varaibles >" + "\r\n" +
                                                    "CREATE TABLE" + "\r\n" +
                                                    "DROP TABLE/PROCEDURE/FUNCTION < tabla >" + "\r\n" +
                                                    "ALTER TABLE < tabla > < definicion >" + "\r\n" +
                                                    "SQL: " + querySQL + "\n" + this.rutaDDBB;
                                                log.end(null, err);
                                                log.Dispose();

                                                throw new ArgumentException(err);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (querySQL.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                    }
                    else
                    {
                        bool retorno = ExecuteCommand(querySQL);      
                        if (this.LogError) log.end(retorno, this.rutaDDBB);
                        log.Dispose();
                        return retorno;
                    }

                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Comprobar si hay algun registro coincidente. La primera columna debe de ser un valor numerico
        /// </summary>
        /// <param name="strSQL">La primera columna debe de ser un valor numerico</param>
        /// <returns></returns>
        public bool ExisteEnDDBB(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ExisteEnDDBB(querySQL)", querySQL, "");
            bool retorno = false;
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(querySQL))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (querySQL.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else
                    {
                        //comprobar si la secuencia SQL devuelve algún valor

                        //int numReg = 0;

                        //realizar las acciones requeridas de la consulta SQL
                        using (SqlConnection con = new SqlConnection(rutaDDBB))
                        {
                            try
                            {
                                con.Open();
                                SqlDataAdapter da = new SqlDataAdapter(querySQL, con);
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                da.Dispose();
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    retorno = true;
                                }
                                else
                                {
                                    retorno = false;
                                }
                                ds.Dispose();
                            }
                            catch (Exception ex)
                            {
                                con.Close();
                                log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                                log.Dispose();

                                throw ex;
                            }
                            con.Close();
                        }
                    }
                }
                else
                    retorno = false;
            }
            if (this.LogError) log.end(retorno, this.rutaDDBB);
            log.Dispose();

            return retorno;
        }

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataSet ConsultarConDataSet(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConDataSet", querySQL, "");
            // Que no sea una cadena vacía
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                // Comprobar que están indicando valores correctos (o casi)
                if (checkQuery(querySQL))
                {
                    bool ok = true;
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (querySQL.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else ok = true;

                    // Comprobar que realmente se use SELECT, o EXEC
                    if (querySQL.ToUpper().IndexOf("EXEC") < 0)
                    {
                        ok = false;
                    }
                    // Comprobar que realmente se use SELECT, o EXEC
                    if (querySQL.ToUpper().IndexOf("SELECT") < 0 && ok == false)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables. SQL: " + querySQL);
                    }
                    SqlConnection con = new SqlConnection(this.rutaDDBB);
                    SqlDataAdapter da = new SqlDataAdapter(querySQL, con);
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        da.Dispose();
                        da = null;
                        ds.Dispose();
                        ds = null;
                        con.Close();
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();

                        throw ex;
                    }
                    da.Dispose();
                    da = null;
                    con.Close();
                    if (this.LogError) log.end(ds, this.rutaDDBB);
                    log.Dispose();

                    return ds;
                }
                else
                {
                    log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
                    log.Dispose();

                    return null;
                }
            }
        }

        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataTable ConsultarConDataTable(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConDataTable", querySQL, "");
            return ConsultarConDataSet(querySQL).Tables[0];
        }


        /// <summary>
        /// Devuelve datos de la consulta
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// Devuelve los datos de la consulta en un DataSet
        /// Si hay error devuelve el mensaje con el error
        /// </returns>
        public DataView ConsultarConDataView(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConDataView", querySQL, "");
            return ConsultarConDataTable(querySQL).DefaultView;
        }

        /// <summary>
        /// Devuelve datos de la consulta en formato XML
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>Devuelve los datos en formato XML, Si hay error devuelve el texto del error</returns>
        public string ConsultarConXML(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsultarConXML", querySQL, "");
            // Que no sea una cadena vacía
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula.\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula.");
            }
            if (checkQuery(querySQL))
            {
                //
                // Comprobar que están indicando valores correctos (o casi)
                //

                bool ok = true;
                // no permitir comentarios ni algunas instrucciones maliciosas
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (querySQL.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("EXEC") < 0) // Comprobar que realmente se use SELECT, o EXEC
                {
                    ok = false;
                }
                else ok = true;

                // Comprobar que realmente se use SELECT, o EXEC
                if (querySQL.ToUpper().IndexOf("SELECT") < 0 && ok == false)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables.SQL: " + querySQL + "\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables. SQL: " + querySQL);
                }
                else
                {
                    SqlConnection con = new SqlConnection(rutaDDBB);
                    SqlDataAdapter da = new SqlDataAdapter(querySQL, con);

                    DataSet ds = new DataSet();

                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        da.Dispose();
                        ds.Dispose();
                        con.Close();
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();

                        throw ex;
                    }
                    da.Dispose();
                    con.Close();
                    if (this.LogError) log.end(ds, this.rutaDDBB);
                    log.Dispose();

                    return ds.GetXml();
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
                log.Dispose();

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
            string querySQL = "SELECT IDENT_CURRENT('" + Tabla + "')";
            defLog log = new defLog(this.FolderLog);
            log.start("ObtenerNuevoId(tabla)", querySQL, Tabla);
            if (checkQuery(querySQL))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (querySQL.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else
                {
                    //cargar las listas del blog con los datos de la base de datos MySQL                
                    int newId = 0;

                    try
                    {
                        string dato = ObtenerDato(querySQL, 0);
                        //if (string.IsNullOrEmpty(dato)) dato = "0";
                        newId = Convert.ToInt32(dato);
                        newId++;
                    }
                    catch (Exception ex)
                    {
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();

                        throw ex;
                    }
                    if (this.LogError) log.end(newId, this.rutaDDBB);
                    log.Dispose();

                    return newId;
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
                log.Dispose();

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
            string querySQL = "SELECT TOP 1 " + col + " FROM " + Tabla +
                " ORDER BY " + col + " DESC";
            defLog log = new defLog(this.FolderLog);
            log.start("ObtenerNuevoId(tabla, col)", querySQL, Tabla + "," + col);
            if (checkQuery(querySQL))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (querySQL.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else
                {
                    int newId = 0;
                    try
                    {
                        string dato = ObtenerDato(querySQL, 0);
                        if (string.IsNullOrEmpty(dato)) newId = 0;
                        else newId = Convert.ToInt32(dato);
                        newId++;
                    }
                    catch (Exception ex)
                    {
                        log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();

                        throw ex;
                    }

                    if (this.LogError) log.end(newId, this.rutaDDBB);
                    log.Dispose();

                    return newId;
                }
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
                log.Dispose();

                return -1;      //devuelve un valor que al intentar usar como ID no se puede ya que ha habido un intento que ataque por injeccion de codigo
            }
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL)
        {
            if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                querySQL = "EXEC " + querySQL;

            return ConsultarConDataSet(querySQL);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return spConDataset(querySQL + " " + param);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver un DataSet
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public DataSet spConDataset(string querySQL, string[] param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return spConDataset(querySQL + " " + doParam);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string querySQL)
        {
            if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                querySQL = "EXEC " + querySQL;

            return ConsultarConDataTable(querySQL);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string querySQL, string param)
        {
            return spConDataTable(querySQL + " " + param);
        }

        /// <summary>
        ///  Ejecutar un SP en el servidor y devolver la primera tabla como DataTable
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public DataTable spConDataTable(string querySQL, string[] param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return spConDataTable(querySQL + " " + doParam);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor devuelve XML con los datos
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string querySQL)
        {
            if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                querySQL = "EXEC " + querySQL;

            return ConsultarConXML(querySQL);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string querySQL, string param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return EjecutarSP(querySQL + " " + param);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public string EjecutarSP(string querySQL, string[] param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return EjecutarSP(querySQL + " " + doParam);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string querySQL)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("ConsutarConDataSet", querySQL, "");
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula.");
            }

            if (checkQuery(querySQL))
            {
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (querySQL.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                }
                else
                {
                    if ((querySQL.ToUpper().IndexOf("EXEC ") < 0))
                        querySQL = "EXEC " + querySQL;

                    SqlConnection cnn = new SqlConnection(rutaDDBB);
                    SqlCommand cmd = new SqlCommand(querySQL, cnn);

                    string datoRetorno = string.Empty;

                    cnn.Open();
                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();         // ejecutar el comando SQL
                        if (dr.Read())                                      // leer los datos
                            datoRetorno = dr.GetValue(0).ToString();        // obtener el campo deseado               
                        else
                            datoRetorno = string.Empty;
                        dr.Close();                                     // cerrar la consulta
                        dr.Dispose();                                   // cerrar la conexión
                    }
                    catch (Exception exReader)
                    {
                        log.end(null, exReader.ToString() + "\n" + this.rutaDDBB);
                        log.Dispose();

                        cmd.Dispose();              // cerrar la conexión
                        cnn.Close();
                        throw exReader;
                    }
                    if (this.LogError) log.end(datoRetorno, this.rutaDDBB);
                    log.Dispose();

                    cmd.Dispose();              // cerrar la conexión
                    cnn.Close();
                    return datoRetorno;
                }
            }
            else
                return string.Empty;

        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados por</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string querySQL, string param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            return escalarSP(querySQL + " " + param);
        }

        /// <summary>
        /// Ejecutar un SP en el servidor
        /// </summary>
        /// <param name="querySQL">Consulta SQL a ejecutar</param>
        /// <param name="param">Parametros del proceso separados en array string</param>
        /// <returns>
        /// </returns>
        public string escalarSP(string querySQL, string[] param)
        {
            //
            // Comprobar que están indicando valores correctos (o casi)
            //
            // Que no sea una cadena vacía
            if (string.IsNullOrEmpty(querySQL) || string.IsNullOrWhiteSpace(querySQL))
            {
                throw new ArgumentException("La cadena no puede ser nula.");
            }

            string doParam = "";

            foreach (string item in param)
            {
                doParam += "'" + item.Replace("'", "''") + "', ";
            }

            doParam = doParam.TrimEnd().Remove(doParam.Length - 2, 1);

            return escalarSP(querySQL + " " + doParam);
        }

        /// <summary>
        /// Obtener los datos de una consulta sql en formato ArrayList
        /// </summary>
        /// <param name="querySQL">Consulta a ejecutar</param>
        /// <returns></returns>
        public ArrayList getData(string querySQL)
        {
            ArrayList retorno = new ArrayList();
            defLog log = new defLog(this.FolderLog);
            log.start("getData(querySQ)", querySQL, "");
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                try
                {
                    DataSet ds = ConsultarConDataSet(querySQL);

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
                    log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                    log.Dispose();

                    throw ex;
                }
            }
            if (this.LogError) log.end(retorno, this.rutaDDBB);
            log.Dispose();

            return retorno;
        }

        /// <summary>
        /// Obtener los datos de una consulta sql en formato ArrayList
        /// </summary>
        /// <param name="querySQL">Consulta a ejecutar</param>
        /// <returns></returns>
        public ArrayList getData(string querySQL, int cols)
        {
            defLog log = new defLog(this.FolderLog);
            log.start("GetData(querySQL, cols)", querySQL, cols.ToString());
            ArrayList retorno = new ArrayList();
            if (string.IsNullOrWhiteSpace(querySQL))
            {
                log.end(null, "La cadena no puede ser nula\n" + this.rutaDDBB);
                log.Dispose();

                throw new ArgumentException("La cadena no puede ser nula");
            }
            else
            {
                if (checkQuery(querySQL))
                {
                    // no permitir comentarios ni algunas instrucciones maliciosas
                    if (querySQL.IndexOf("--") > -1)
                    {
                        log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP TABLE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else if (querySQL.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                    {
                        log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);
                        log.Dispose();

                        throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + querySQL);
                    }
                    else
                    {
                        try
                        {
                            using (SqlConnection con = new SqlConnection(rutaDDBB))
                            {
                                SqlCommand cmd = new SqlCommand(querySQL, con);
                                SqlDataReader ds;

                                con.Open();

                                //localizar los datos en la base de datos
                                ds = cmd.ExecuteReader();
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

                                ds.Close();
                                con.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.end(null, ex.ToString() + "\n" + this.rutaDDBB);
                            log.Dispose();

                            throw ex;

                        }

                        if (this.LogError) log.end(retorno, this.rutaDDBB);
                        log.Dispose();

                        return retorno;
                    }
                }
                else
                {
                    log.end(null, "No ha superado la prueba de validacion\n" + this.rutaDDBB);
                    log.Dispose();

                    throw new ArgumentException("La cadena no puede ser nula");
                }
            }
        }

    }
}