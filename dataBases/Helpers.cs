using drualcman.Attributes;
using drualcman.Enums;
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
    /// <summary>
    /// Setup inner model relationship
    /// </summary>
    public record TableName(string Name, string Short, InnerDirection Inner, string Column, string Index);

    public partial class dataBases
    {
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

        /// <summary>
        /// Get the select from the properties name about the model send
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SetQuery<TModel>()
        {
            PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            List<TableName> tables = new List<TableName>();
            string tableName;
            DatabaseAttribute table = typeof(TModel).GetCustomAttribute<DatabaseAttribute>();
            if (table is not null)
            {
                if (string.IsNullOrEmpty(table.Name)) tableName = typeof(TModel).Name;
                else tableName = table.Name;
            }
            else tableName = typeof(TModel).Name;
            int tableCount = 0;
            TableName newTable = new TableName(tableName, $"t{tableCount}", InnerDirection.NONE, string.Empty, string.Empty);
            tables.Add(newTable);

            StringBuilder retorno = new StringBuilder("SELECT ");
            int c = properties.Length;
            for (int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                string fieldName;
                if (field is not null)
                {
                    if (!field.Ignore)
                    {
                        if (field.Inner != InnerDirection.NONE)
                        {
                            //add columns from the property model
                            InnerColumns(properties[i], ref retorno, field, ref tables, ref tableCount);
                            fieldName = string.Empty;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(field.Name))
                                fieldName = properties[i].Name;
                            else
                                fieldName = field.Name;
                        }
                    }
                    else
                        fieldName = string.Empty;
                }
                else
                    fieldName = properties[i].Name;
                if (!string.IsNullOrEmpty(fieldName))
                    retorno.Append($" {tables[0].Short}.[{fieldName}] [{tables[0].Short}.{fieldName}],");
                
            }
            retorno.Remove(retorno.Length - 1, 1);
            retorno.Append($" FROM [{tables[0].Name}] {tables[0].Short} ");

            if (tables.Count() > 1)
            {
                //add inner joins depending of the model database attributes
                int tc = tables.Count();
                for (int i = 1; i < tc; i++)
                {
                    retorno.Append($" {tables[i].Inner} JOIN [{tables[i].Name}] {tables[i].Short} on {tables[0].Short}.{(string.IsNullOrEmpty(tables[i].Index) ? string.IsNullOrEmpty(tables[i].Column) ? $"{tables[i].Name}Id" : tables[i].Column : tables[i].Index)} = {tables[i].Short}.{(string.IsNullOrEmpty(tables[i].Column) ? $"{tables[i].Name}Id" : tables[i].Column)}");
                }
            }

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
                        if (!string.IsNullOrEmpty(field.Name)) fieldName = field.Name;
                        if (field.IndexKey)
                        {
                            if (string.IsNullOrEmpty(field.IndexedName)) 
                            {
                                if (this.WhereRequired.ContainsKey(fieldName))
                                {
                                    foundSome = true;
                                    KeyValuePair<string, object> where = this.WhereRequired.Where(k => k.Key == fieldName).FirstOrDefault();
                                    retorno.Append($" {tables[0].Short}.[{fieldName}] = {where.Value} ");
                                    retorno.Append("AND");
                                }
                            }
                            else
                            {
                                if (this.WhereRequired.ContainsKey(field.IndexedName))
                                {
                                    foundSome = true;
                                    KeyValuePair<string, object> where = this.WhereRequired.Where(k => k.Key == field.IndexedName).FirstOrDefault();
                                    retorno.Append($" {tables[0].Short}.[{fieldName}] = {where.Value} ");
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
                            retorno.Append($" {tables[0].Short}.[{fieldName}] = {where.Value} ");
                            retorno.Append("AND");
                        }
                    }
                }
                if (foundSome) retorno.Remove(retorno.Length - 3, 3);
                else retorno.Remove(retorno.Length - 7, 7);
            }
            return retorno.ToString(); 
        }

        private void InnerColumns(PropertyInfo column, ref StringBuilder retorno, DatabaseAttribute origin, ref List<TableName> tables, ref int tableCount)
        {
            Type t = column.PropertyType;
            string tableName;
            string shortName;
            if (Helpers.ObjectHelpers.IsGenericList(column.PropertyType.FullName))
            {
                PropertyInfo[] fields = column.PropertyType.GetGenericArguments()[0].GetProperties();
                DatabaseAttribute table = column.PropertyType.GetGenericArguments()[0].GetCustomAttribute<DatabaseAttribute>();
                if (table is not null)
                {
                    if (string.IsNullOrEmpty(table.Name))
                        tableName = column.PropertyType.GetGenericArguments()[0].Name;
                    else
                        tableName = table.Name;
                }
                else
                    tableName = column.PropertyType.GetGenericArguments()[0].Name;

                tableCount++;
                shortName = $"t{tableCount}";
                TableName newTable = new TableName(tableName, shortName, origin.Inner,
                    origin.InnerColumn ?? origin.Name ?? "", origin.InnerIndex ?? origin.Name ?? origin.InnerColumn ?? "");
                tables.Add(newTable);

                InnerColumns(fields, tableName, shortName, ref retorno, ref tables, ref tableCount);
            }
            else
            {
                PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);                
                DatabaseAttribute table = t.GetCustomAttribute<DatabaseAttribute>();
                if (table is not null)
                {
                    if (string.IsNullOrEmpty(table.Name))
                        tableName = t.Name;
                    else
                        tableName = table.Name;
                }
                else
                    tableName = t.Name;

                tableCount++;
                shortName = $"t{tableCount}";
                TableName newTable = new TableName(tableName, shortName, origin.Inner,
                    origin.InnerColumn ?? origin.Name ?? "", origin.InnerIndex ?? origin.Name ?? origin.InnerColumn ?? "");
                tables.Add(newTable);

                InnerColumns(properties, tableName, shortName, ref retorno, ref tables, ref tableCount);
            }
        }

        private void InnerColumns(PropertyInfo[] properties, string tableName, string shortName, ref StringBuilder retorno, ref List<TableName> tables, ref int tableCount)
        {            
            int c = properties.Length;
            for (int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                string fieldName = properties[i].Name;
                if (field is not null)
                {
                    if (!field.Ignore)
                    {
                        if (field.Inner != InnerDirection.NONE)
                        {
                            //add columns from the property model
                            InnerColumns(properties[i], ref retorno, field, ref tables, ref tableCount);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(field.Name))
                                fieldName = properties[i].Name;
                            else
                                fieldName = field.Name;
                        }
                    }
                    else
                        fieldName = string.Empty;
                }
                if (!string.IsNullOrEmpty(fieldName))
                    retorno.Append($" {shortName}.[{fieldName}] [{shortName}.{fieldName}],");

            }
        }
    }
}
