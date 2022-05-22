using drualcman.Attributes;
using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace drualcman.Data
{
    public class TableNamesHelper
    {
        private List<TableName> tableNamesBK = new List<TableName>();
        public IEnumerable<TableName> TableNames => tableNamesBK;

        public void AddTableNames<TModel>()
        {
            tableNamesBK = new List<TableName>();
            PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string tableName;
            DatabaseAttribute table = typeof(TModel).GetCustomAttribute<DatabaseAttribute>();
            if(table is not null)
            {
                if(string.IsNullOrEmpty(table.Name)) tableName = typeof(TModel).Name;
                else tableName = table.Name;
            }
            else tableName = typeof(TModel).Name;
            int tableCount = 0;
            string shortName = $"t{tableCount}";
            TableName newTable = new TableName(tableName, shortName, string.Empty, InnerDirection.NONE, string.Empty, string.Empty, typeof(TModel).Name);
            tableNamesBK.Add(newTable);

            int c = properties.Length;
            for(int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                if(field is not null)
                {
                    if(!field.Ignore)
                    {
                        if(field.Inner != InnerDirection.NONE)
                        {
                            //add columns from the property model
                            AddTable(properties[i], field, ref tableCount, shortName);
                        }
                    }
                }
            }
        }

        private void AddTable(PropertyInfo column, DatabaseAttribute origin,
            ref int tableCount, string shortReference)
        {
            Type t = column.PropertyType;
            string tableName;
            string shortName;
            PropertyInfo[] properties;
            DatabaseAttribute table;
            if(drualcman.Helpers.ObjectHelpers.IsGenericList(column.PropertyType.FullName))
            {
                properties = column.PropertyType.GetGenericArguments()[0].GetProperties();
                table = column.PropertyType.GetGenericArguments()[0].GetCustomAttribute<DatabaseAttribute>();
                if(table is not null && !string.IsNullOrEmpty(table.Name)) tableName = table.Name;
                else tableName = column.PropertyType.GetGenericArguments()[0].Name;
            }
            else
            {
                properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                table = t.GetCustomAttribute<DatabaseAttribute>();
                if(table is not null && !string.IsNullOrEmpty(table.Name)) tableName = table.Name;
                else tableName = t.Name;
            }

            tableCount++;
            shortName = $"t{tableCount}";
            TableName newTable = new TableName(string.IsNullOrEmpty(origin.IndexedName) ? tableName : origin.IndexedName, shortName, shortReference, origin.Inner,
                origin.InnerColumn ?? origin.Name ?? "", origin.InnerIndex ?? origin.Name ?? origin.InnerColumn ?? "", t.Name, column);
            tableNamesBK.Add(newTable);
            AddTable(properties, shortName, ref tableCount);
        }

        private void AddTable(PropertyInfo[] properties, string shortName, ref int tableCount)
        {
            int c = properties.Length;
            for(int i = 0; i < c; i++)
            {
                DatabaseAttribute field = properties[i].GetCustomAttribute<DatabaseAttribute>();
                if(field is not null)
                {
                    if(!field.Ignore)
                    {
                        if(field.Inner != InnerDirection.NONE)
                        {
                            //add columns from the property model
                            AddTable(properties[i], field, ref tableCount, shortName);
                        }
                    }
                }
            }
        }
    }
}
