using drualcman.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace drualcman
{
    public partial class dataBases
    {
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

        /// <summary>
        /// Get the select from the properties name about the model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SetQuery<TModel>()
        {
            PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);  
            string tableName;
            DatabaseAttribute table = typeof(TModel).GetCustomAttribute<DatabaseAttribute>();
            if (table is not null)
            {
                if (string.IsNullOrEmpty(table.Name)) tableName = typeof(TModel).Name;
                else tableName = table.Name;
            }
            else tableName = typeof(TModel).Name;

            StringBuilder retorno = new StringBuilder("SELECT ");
            int c = properties.Length;
            for (int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                string fieldName = properties[i].Name;
                if (field is not null)
                {
                    if (!field.Ignore)
                    {
                        if (string.IsNullOrEmpty(field.Name)) fieldName = properties[i].Name;
                        else fieldName = field.Name;
                    }
                    else fieldName = string.Empty;
                }
                if (!string.IsNullOrEmpty(fieldName))
                    retorno.Append($" [{fieldName}],");
                
            }
            retorno.Remove(retorno.Length - 1, 1);
            retorno.Append($" FROM [{tableName}] ");

            if (this.WhereRequired is not null)
            {
                bool foundSome = false;
                retorno.Append($" WHERE ");
                for (int i = 0; i < c; i++)
                {
                    DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                    string fieldName = properties[i].Name;
                    if (field is not null)
                    {
                        if (field.IndexKey)
                        {
                            if (string.IsNullOrEmpty(field.IndexedName)) 
                            {
                                if (this.WhereRequired.ContainsKey(fieldName))
                                {
                                    foundSome = true;
                                    KeyValuePair<string, object> where = this.WhereRequired.Where(k => k.Key == fieldName).FirstOrDefault();
                                    retorno.Append($" [{where.Key}] = {where.Value} ");
                                    retorno.Append("AND");
                                }
                            }
                            else
                            {
                                fieldName = field.IndexedName;
                                if (this.WhereRequired.ContainsKey(fieldName))
                                {
                                    foundSome = true;
                                    KeyValuePair<string, object> where = this.WhereRequired.Where(k => k.Key == fieldName).FirstOrDefault();
                                    retorno.Append($" [{where.Key}] = {where.Value} ");
                                    retorno.Append("AND");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.WhereRequired.ContainsKey(fieldName))
                        {
                            foundSome = true;
                            KeyValuePair<string, object> where = this.WhereRequired.Where(k => k.Key == fieldName).FirstOrDefault();
                            retorno.Append($" [{where.Key}] = {where.Value} ");
                            retorno.Append("AND");
                        }
                    }
                }
                if (foundSome) retorno.Remove(retorno.Length - 3, 3);
                else retorno.Remove(retorno.Length - 7, 7);
            }
            string k = retorno.ToString();
            return retorno.ToString(); 
        }
    }
}
