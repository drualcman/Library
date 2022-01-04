using drualcman.Data;
using System;
using System.Text.RegularExpressions;

namespace drualcman
{
    public partial class dataBases
    {
        public string SetQuery<TModel>()
        {
            SqlQueryTranslator queryTranslator = new SqlQueryTranslator(this.WhereRequired);
            return queryTranslator.SetQuery<TModel>();
        }

        protected string CleanSqlDataColumns(string input)
        {
            string pattern = "\\[t[0-9].";
            string replacement = "[";
            return Regex.Replace(input, pattern, replacement);
        }

        #region security
        private void CheckSqlInjection(string query, defLog log)
        {
            if (checkQuery(query))
            {
                bool ok = true;
                // no permitir comentarios ni algunas instrucciones maliciosas
                if (query.IndexOf("--") > -1)
                {
                    log.end(null, "No se admiten comentarios de SQL en la cadena de selección\n" + this.rutaDDBB);

                    throw new ArgumentException("No se admiten comentarios de SQL en la cadena de selección. SQL: " + query);
                }
                else if (query.ToUpper().IndexOf("DROP TABLE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + query);
                }
                else if (query.ToUpper().IndexOf("DROP PROCEDURE ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + query);
                }
                else if (query.ToUpper().IndexOf("DROP FUNCTION ") > -1)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados...\n" + this.rutaDDBB);

                    throw new ArgumentException("La cadena debe ser SELECT campos FROM tabla, no DROP y otros comandos no adecuados... SQL: " + query);
                }
                else ok = true;

                // Comprobar que realmente se use SELECT, o EXEC
                if (query.ToUpper().IndexOf("EXEC") < 0)
                {
                    ok = false;
                }
                // Comprobar que realmente se use SELECT, o EXEC
                if (query.ToUpper().IndexOf("SELECT") < 0 && ok == false)
                {
                    log.end(null, "La cadena debe ser SELECT campos FROM tabla / EXEC Storage Proces and variables\n" + this.rutaDDBB);

                    throw new ArgumentException("Query must be SELECT fields FROM table / EXEC Storage Process and variables. SQL: " + query);
                }
                else log.end(null, "Ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
            }
            else
            {
                log.end(null, "No ha superado la prueba de comando correcto.\n" + this.rutaDDBB);
                throw new ArgumentException("Query must be SELECT fields FROM table / EXEC Storage Process and variables. SQL: " + query);
            }
        }

        /// <summary>
        /// Prevenir INJECTION SQL en las consultas
        /// </summary>
        /// <param name="query">consulta enviada</param>
        /// <returns></returns>
        private bool checkQuery(string query)
        {
            bool resultado;
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
        #endregion
    }
}
