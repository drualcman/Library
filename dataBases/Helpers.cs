using drualcman.Attributes;
using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace drualcman
{
    /// <summary>
    /// Setup inner model relationship
    /// </summary>
    public record TableName(string Name, string ShortName, 
        string ShortReference, InnerDirection Inner, string Column, 
        string InnerIndex, string ClassName, PropertyInfo Instance = null);

    public partial class dataBases
    {
        private List<TableName> TableNames;

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

            AddTableNames<TModel>();
            string shortName = TableNames[0].ShortName;

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
                            InnerColumns(properties[i], retorno, field, shortName);
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
                    retorno.Append($" {TableNames[0].ShortName}.[{fieldName}] [{TableNames[0].ShortName}.{fieldName}],");
                
            }
            retorno.Remove(retorno.Length - 1, 1);
            retorno.Append(Environment.NewLine);
            retorno.Append($"FROM [{TableNames[0].Name}] {TableNames[0].ShortName} ");
            if (TableNames.Count() > 1)
            {
                //add inner joins depending of the model database attributes
                int tc = TableNames.Count();
                for (int i = 1; i < tc; i++)
                {
                    retorno.Append(Environment.NewLine);
                    retorno.Append($"\t{TableNames[i].Inner} JOIN [{TableNames[i].Name}] {TableNames[i].ShortName} on {TableNames[i].ShortReference}.{(string.IsNullOrEmpty(TableNames[i].InnerIndex) ? string.IsNullOrEmpty(TableNames[i].Column) ? $"{TableNames[i].Name}Id" : TableNames[i].Column : TableNames[i].InnerIndex)} = {TableNames[i].ShortName}.{(string.IsNullOrEmpty(TableNames[i].Column) ? $"{TableNames[i].Name}Id" : TableNames[i].Column)}");
                }
            }

            retorno.Append(Environment.NewLine);
            if (this.WhereRequired is not null)
            {
                bool foundSome = false;
                retorno.Append($"WHERE ");
                for (int i = 0; i < c; i++)
                {
                    DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                    string fieldName = properties[i].Name;
                    if (field is not null)
                    {
                        if (!string.IsNullOrEmpty(field.Name)) fieldName = field.Name;
                        if (field.IndexKey)
                        {
                            if (string.IsNullOrEmpty(field.IndexedName) && this.WhereRequired.ContainsKey(fieldName))
                            {
                                foundSome = true;
                                retorno.Append($" {TableNames[0].ShortName}.[{fieldName}] = {this.GetWhereValue(fieldName)} ");
                                retorno.Append("AND");
                            }
                            else
                            {
                                if (this.WhereRequired.ContainsKey(field.IndexedName))
                                {
                                    foundSome = true;
                                    retorno.Append($" {TableNames[0].ShortName}.[{fieldName}] = {this.GetWhereValue(field.IndexedName)} ");
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
                            retorno.Append($" {TableNames[0].ShortName}.[{fieldName}] = {this.GetWhereValue(fieldName)} ");
                            retorno.Append("AND");
                        }
                    }
                }
                if (foundSome) retorno.Remove(retorno.Length - 3, 3);
                else retorno.Remove(retorno.Length - 7, 7);
            }
            return retorno.ToString(); 
        }

        private void InnerColumns(PropertyInfo column, StringBuilder retorno, 
            DatabaseAttribute origin, string shortReference)
        {
            Type t = column.PropertyType;
            PropertyInfo[] properties;
            DatabaseAttribute table;
            if (Helpers.ObjectHelpers.IsGenericList(column.PropertyType.FullName))
            {
                properties = column.PropertyType.GetGenericArguments()[0].GetProperties();
                table = column.PropertyType.GetGenericArguments()[0].GetCustomAttribute<DatabaseAttribute>();
            }
            else
            {
                properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);                
                table = t.GetCustomAttribute<DatabaseAttribute>();
            }

            TableName father = TableNames.Find(r => r.Name == column.PropertyType.Name);
            InnerColumns(properties, father?.ShortName, retorno);
        }

        private void InnerColumns(PropertyInfo[] properties, string shortName, StringBuilder retorno)
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
                            InnerColumns(properties[i], retorno, field, shortName);
                            fieldName = string.Empty;
                        }
                        else
                        {
                            if (Helpers.ObjectHelpers.IsGenericList(properties[i].PropertyType.FullName))
                                fieldName = string.Empty;
                            else
                            {
                                if (string.IsNullOrEmpty(field.Name))
                                    fieldName = properties[i].Name;
                                else
                                    fieldName = field.Name;
                            }
                        }
                    }
                    else
                        fieldName = string.Empty;
                }
                if (!string.IsNullOrEmpty(fieldName))
                    retorno.Append($" {shortName}.[{fieldName}] [{shortName}.{fieldName}],");
            }
        }

        protected string CleanSqlDataColumns(string input)
        {
            string pattern = "\\[t[0-9].";
            string replacement = "[";
            return Regex.Replace(input, pattern, replacement);
        }
    }
}
