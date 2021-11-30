using drualcman.Attributes;
using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Helpers
{
    internal class ColumnsNames
    {

        readonly ReadOnlyCollection<string> Columns;
        readonly ReadOnlyCollection<TableName> Tables;

        public ColumnsNames(IList<string> cols, List<TableName> tables)
        {
            Columns = new ReadOnlyCollection<string>(cols);
            Tables = new ReadOnlyCollection<TableName>(tables);
        }

        public ColumnsNames(string[] columnNames, List<TableName> tables)
            : this(columnNames.ToList(), tables) { }

        public ColumnsNames(ReadOnlyCollection<DbColumn> columns, List<TableName> tables) 
        {
            List<string> cols = new List<string>();
            foreach (DbColumn item in columns)
            {
                cols.Add(item.ColumnName.ToLower());
            }
            Columns = new ReadOnlyCollection<string>(cols);
            Tables = new ReadOnlyCollection<TableName>(tables);
        }

        /// <summary>
        /// Get all the columns properties need from the query used
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<Columns> HaveColumns(Type model, string shortName)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            bool isDirectQuery = Columns[0].IndexOf(".") < 0;
            int c = properties.Length;

            for (int propertyIndex = 0; propertyIndex < c; propertyIndex++)
            {
                string columnName;

                if (properties[propertyIndex].PropertyType.IsClass && 
                    !properties[propertyIndex].PropertyType.IsArray &&
                    properties[propertyIndex].PropertyType != typeof(string))
                {
                    result.AddRange(HaveColumns(properties[propertyIndex].PropertyType, shortName));
                }
                else
                {
                    if (isDirectQuery) columnName = properties[propertyIndex].Name.ToLower();
                    else columnName = $"{shortName}.{properties[propertyIndex].Name}".ToLower();
                    if (Columns.Contains(columnName))
                    {
                        result.Add(SetColumn(properties[propertyIndex], shortName, columnName));
                    }
                }

                
            }
            return result;
        }

        private Columns SetColumn(PropertyInfo property, string shortName, string columnName)
        {
            string propertyType;
            if (property.PropertyType.Name == typeof(bool).Name) propertyType = "bool";
            else if (property.PropertyType.Name == typeof(int).Name) propertyType = "int";
            else if (property.PropertyType.Name == typeof(long).Name) propertyType = "long";
            else if (property.PropertyType.Name == typeof(double).Name) propertyType = "double";
            else if (property.PropertyType.Name == typeof(decimal).Name) propertyType = "decimal";
            else if (property.PropertyType.Name == typeof(float).Name) propertyType = "float";
            else if (property.PropertyType.Name == typeof(short).Name) propertyType = "short";
            else if (property.PropertyType.Name == typeof(byte).Name) propertyType = "byte";
            else if (property.PropertyType.Name == typeof(DateTime).Name) propertyType = "date";
            else if (property.PropertyType.Name == typeof(Nullable).Name) propertyType = "nullable";
            else if (property.PropertyType.Name == typeof(Nullable<>).Name) propertyType = "nullable";
            else propertyType = "text";

            TableName table = Tables.Where(t => t.ShortName == shortName).FirstOrDefault();
            DatabaseAttribute options = property.GetCustomAttribute<DatabaseAttribute>();

            return new Columns
            {
                Column = property,
                Options = options,
                TableShortName = shortName,
                ColumnName = columnName,
                PropertyType = propertyType,
                TableIndex = Array.IndexOf(Tables.ToArray(), table)
            };
        }

    }
}
