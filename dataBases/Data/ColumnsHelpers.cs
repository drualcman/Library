using drualcman.Attributes;
using drualcman.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace drualcman.Data
{
    class ColumnsHelpers
    {
        public TModel ColumnToObject<TModel>(DataRow row, Type model, List<TableName> tables, int tableCount, List<string> hasList, List<Columns> columns) where TModel : new()
        {
            //var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);
            TModel item = new TModel();

            InstanceProperties(item, tables, hasList);

            int c = columns.Count;

            for (int index = 0; index < c; index++)
            {
                if (columns[index].Options is not null)
                {
                    if (!columns[index].Options.Ignore)
                    {
                        if (Helpers.ObjectHelpers.IsGenericList(columns[index].Column.PropertyType.FullName) &&
                                        !hasList.Contains(columns[index].Column.PropertyType.Name))
                        {
                            hasList.Add(columns[index].Column.PropertyType.Name);
                            Type[] genericType = columns[index].Column.PropertyType.GetGenericArguments();
                            Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                            object dat = Activator.CreateInstance(creatingCollectionType);
                            //mutexColumn.WaitOne();
                            columns[index].Column.SetValue(item, dat);
                            //mutexColumn.ReleaseMutex();
                        }
                        else
                        {
                            //mutexColumn.WaitOne();
                            SetValue(columns[index], item, tables, row);
                            //mutexColumn.ReleaseMutex();
                        }
                    }
                }
                else
                {
                    //mutexColumn.WaitOne();
                    SetValue(columns[index], item, tables, row);
                    //mutexColumn.ReleaseMutex();
                }
            }


            //ConcurrentQueue<int> columnQueue = new ConcurrentQueue<int>(Enumerable.Range(0, columns.Count));
            //CountdownEvent countdownEvent = new CountdownEvent(columnQueue.Count);

            //Action processColumns = () =>
            //{
            //    int index;
            //    while (columnQueue.TryDequeue(out index))
            //    {
            //        countdownEvent.Signal();
            //    }

            //};

            //Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);

            //countdownEvent.Wait();
            //countdownEvent.Dispose();

            return item;
        }

        public TModel ColumnToObject<TModel>(SqlDataReader row, Type model, List<TableName> tables, int tableCount, List<string> hasList, List<Columns> columns) where TModel : new()
        {
            //var item = Assembly.GetAssembly(model).CreateInstance(model.FullName, true);
            TModel item = new TModel();

            InstanceProperties(item, tables, hasList);

            int c = columns.Count;

            for (int index = 0; index < c; index++)
            {
                if (columns[index].Options is not null)
                {
                    if (!columns[index].Options.Ignore)
                    {
                        if (Helpers.ObjectHelpers.IsGenericList(columns[index].Column.PropertyType.FullName) &&
                                        !hasList.Contains(columns[index].Column.PropertyType.Name))
                        {
                            hasList.Add(columns[index].Column.PropertyType.Name);
                            Type[] genericType = columns[index].Column.PropertyType.GetGenericArguments();
                            Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                            object dat = Activator.CreateInstance(creatingCollectionType);
                            //mutexColumn.WaitOne();
                            columns[index].Column.SetValue(item, dat);
                            //mutexColumn.ReleaseMutex();
                        }
                        else
                        {
                            //mutexColumn.WaitOne();
                            SetValue(columns[index], item, tables, row);
                            //mutexColumn.ReleaseMutex();
                        }
                    }
                }
                else
                {
                    //mutexColumn.WaitOne();
                    SetValue(columns[index], item, tables, row);
                    //mutexColumn.ReleaseMutex();
                }
            }


            //ConcurrentQueue<int> columnQueue = new ConcurrentQueue<int>(Enumerable.Range(0, columns.Count));
            //CountdownEvent countdownEvent = new CountdownEvent(columnQueue.Count);

            //Action processColumns = () =>
            //{
            //    int index;
            //    while (columnQueue.TryDequeue(out index))
            //    {
            //        countdownEvent.Signal();
            //    }

            //};

            //Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);
            ////Task.Run(processColumns);

            //countdownEvent.Wait();
            //countdownEvent.Dispose();

            return item;
        }

        /// <summary>
        /// Get all the columns properties need from the query used
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Columns> HaveColumns(ReadOnlyCollection<DbColumn> columns, Type model, string shortName, List<TableName> tableNames)
        {
            List<string> cols = new List<string>();

            foreach (DbColumn item in columns)
            {
                cols.Add(item.ColumnName);
            }

            return HaveColumns(cols.ToArray(), model, shortName, tableNames);
        }

        /// <summary>
        /// Get all the columns properties need from the query used
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<Columns> HaveColumns(string[] columns, Type model, string shortName, List<TableName> tableNames)
        {
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<Columns> result = new List<Columns>();
            int totalDbColumns = columns.Length;
            int propertyCount = properties.Length;
            bool isDirectQuery = columns[0].IndexOf(".") < 0;
            string columnCompare;
            int tableCount = 0;
            int c = properties.Length;

            for (int propertyIndex = 0; propertyIndex < c; propertyIndex++)
            {
                DatabaseAttribute options = properties[propertyIndex].GetCustomAttribute<DatabaseAttribute>();
                int columnIndex = 0;
                bool notFound = true;

                TableName table = tableNames.Where(t => t.ClassName == properties[propertyIndex].ReflectedType.Name).FirstOrDefault();
                do
                {
                    if (options is null)
                    {
                        if (isDirectQuery)
                        {
                            columnCompare = columns[columnIndex];
                        }
                        else
                        {
                            columnCompare = columns[columnIndex].Replace($"{shortName}.", "");
                        }
                    }
                    else if (options.Inner == InnerDirection.NONE)
                    {
                        columnCompare = options.Name == columns[columnIndex].Replace($"{shortName}.", "") ?
                            properties[propertyIndex].Name : columns[columnIndex].Replace($"{shortName}.", "");
                    }
                    else
                    {
                        string referenceShorName = shortName;
                        if (tableNames.Where(t => t.ClassName == properties[propertyIndex].PropertyType.Name).FirstOrDefault() is null)
                        {
                            tableCount = tableNames.Count;
                            table = new TableName(columns[columnIndex], $"t{tableCount}", referenceShorName, options.Inner, options.InnerColumn, options.InnerIndex, properties[propertyIndex].PropertyType.Name, properties[propertyIndex]);
                            tableNames.Add(table);
                        }
                        else
                        {
                            referenceShorName = table.ShortName;
                        }
                        result.AddRange(HaveColumns(columns, properties[propertyIndex].PropertyType, $"t{tableCount}", tableNames));
                        columnCompare = string.Empty;
                    }
                    notFound = !(columnCompare.ToLower() == properties[propertyIndex].Name.ToLower());

                    columnIndex++;
                } while (columnIndex < totalDbColumns && notFound && !string.IsNullOrEmpty(columnCompare));

                if (!notFound)
                {
                    columnIndex--;
                    string propertyType;
                    if (properties[propertyIndex].PropertyType.Name == typeof(bool).Name) propertyType = "bool";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(int).Name) propertyType = "int";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(long).Name) propertyType = "long";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(double).Name) propertyType = "double";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(decimal).Name) propertyType = "decimal";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(float).Name) propertyType = "float";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(short).Name) propertyType = "short";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(byte).Name) propertyType = "byte";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(DateTime).Name) propertyType = "date";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(Nullable).Name) propertyType = "nullable";
                    else if (properties[propertyIndex].PropertyType.Name == typeof(Nullable<>).Name) propertyType = "nullable";
                    else propertyType = "text";

                    Columns col = new Columns { Column = properties[propertyIndex], Options = options, TableShortName = shortName, ColumnName = columns[columnIndex], PropertyType = propertyType, TableIndex = Array.IndexOf(tableNames.ToArray(), table) };
                    //mutex.WaitOne();
                    result.Add(col);
                    //mutex.ReleaseMutex();
                }
            }

            //ConcurrentQueue<int> porpertiesQueue = new ConcurrentQueue<int>(Enumerable.Range(0, properties.Length));
            //CountdownEvent countdownEvent = new CountdownEvent(porpertiesQueue.Count);

            //Action processPoperties = () =>
            //{
            //    int propertyIndex;
            //    while (porpertiesQueue.TryDequeue(out propertyIndex))
            //    {

            //        countdownEvent.Signal();
            //    }
            //};

            //Task.Run(processPoperties);
            //Task.Run(processPoperties);
            //Task.Run(processPoperties);
            //Task.Run(processPoperties);
            //Task.Run(processPoperties);

            //countdownEvent.Wait();
            //countdownEvent.Dispose();
            return result;
        }

        private void InstanceProperties<TModel>(TModel item, List<TableName> tableNames, List<string> hasList)
        {
            int c = tableNames.Count;
            int index;
            for (index = 1; index < c; index++)
            {
                if (tableNames[index].Instance is not null)
                {
                    object property = null;
                    if (Helpers.ObjectHelpers.IsGenericList(tableNames[index].Instance.PropertyType.FullName))
                    {
                        Type[] genericType = tableNames[index].Instance.PropertyType.GetGenericArguments();
                        if (!hasList.Contains(genericType[0].Name))
                        {
                            hasList.Add(genericType[0].Name);
                            Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                            property = Activator.CreateInstance(creatingCollectionType);
                            //get the the name property content the list
                            TableName father = tableNames.Where(t => t.ShortName == tableNames[index].ShortReference).FirstOrDefault();
                            tableNames[index].Instance.SetValue(item.GetPropValue(father.ClassName), property, null);
                        }
                    }
                    else
                    {
                        property = Activator.CreateInstance(tableNames[index].Instance.PropertyType);
                        //mutexColumn.WaitOne();
                        tableNames[index].Instance.SetValue(item, property, null);
                        //mutexColumn.ReleaseMutex();
                    }
                }
            }

            //ConcurrentQueue<int> columnTableQueue = new ConcurrentQueue<int>(Enumerable.Range(0, tables.Count));
            //CountdownEvent countdownTableEvent = new CountdownEvent(columnTableQueue.Count);

            //Action processColumnsInstances = () =>
            //{
            //    int index;
            //    while (columnTableQueue.TryDequeue(out index))
            //    {

            //        countdownTableEvent.Signal();
            //    }
            //};

            //Task.Run(processColumnsInstances);
            //Task.Run(processColumnsInstances);

            //countdownTableEvent.Wait();
            //countdownTableEvent.Dispose();
        }

        private void SetValue(Columns column, object item, List<TableName> tables, DataRow row)
        {
            SetValue(column, item, tables, row[column.ColumnName]);
        }

        private void SetValue(Columns column, object item, List<TableName> tables, SqlDataReader row)
        {
            SetValue(column, item, tables, row[column.ColumnName]);
        }

        private void SetValue(Columns column, object item, List<TableName> tables, object row)
        {
            try
            {
                if (column.TableIndex > 0)
                {
                    SetValue(column.PropertyType, column.Column, item.GetPropValue(tables[column.TableIndex].Instance.Name), row);
                }
                else
                {
                    SetValue(column.PropertyType, column.Column, item, row);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Console.WriteLine("ex 2 {0}", err);
                SetValue(column.Column, item, row);
            }

        }

        private void SetValue(PropertyInfo sender, object destination, object value)
        {
            try
            {
                if (value != null)
                {
                    sender.SetValue(destination, Convert.ChangeType(value, Nullable.GetUnderlyingType(sender.PropertyType)));
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Console.WriteLine("ex 3 {0}", err);
            }

        }

        private void SetValue(string propertyType, PropertyInfo sender, object destination, object value)
        {
            if (value.GetType().Name != typeof(DBNull).Name)
            {
                switch (propertyType)
                {
                    case "bool":
                        if (int.TryParse(value.ToString(), out int test))
                        {
                            if (test == 0) sender.SetValue(destination, false);
                            else sender.SetValue(destination, true);
                        }
                        else sender.SetValue(destination, Convert.ToBoolean(value));
                        break;
                    case "int":
                        sender.SetValue(destination, Convert.ToInt32(value));
                        break;
                    case "double":
                        sender.SetValue(destination, Convert.ToDouble(value));
                        break;
                    case "float":
                        sender.SetValue(destination, Convert.ToSingle(value));
                        break;
                    case "decimal":
                        sender.SetValue(destination, Convert.ToDecimal(value));
                        break;
                    case "long":
                        sender.SetValue(destination, Convert.ToInt64(value));
                        break;
                    case "short":
                        sender.SetValue(destination, Convert.ToInt16(value));
                        break;
                    case "byte":
                        sender.SetValue(destination, Convert.ToByte(value));
                        break;
                    case "date":
                        sender.SetValue(destination, Convert.ToDateTime(value));
                        break;
                    case "nullable":
                        sender.SetValue(destination, value);
                        break;
                    default:
                        sender.SetValue(destination, value.ToString());
                        break;
                }
            }
        }
    }
}
