using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Helpers
{
    internal class ColumnSqlClientToObject
    {
        readonly ColumnsNames Names;
        readonly InstanceModel Model;
        readonly ReadOnlyCollection<TableName> Tables;

        public ColumnSqlClientToObject(ColumnsNames names, InstanceModel model, IList<TableName> tables)
        {
            Names = names;
            Model = model;
            Tables = new ReadOnlyCollection<TableName>(tables);
        }

        public ColumnToObjectResponse SetColumnToObject<TModel>(ColumnValue value, SqlDataReader reader, TModel item, string actualTable)
        {
            ColumnToObjectResponse response = new ColumnToObjectResponse
            {
                ActualTable = actualTable,
                InUse = item
            };

            TableName father = Tables.Where(t => t.ShortName == response.ActualTable).FirstOrDefault();
            Type model = father.Instance?.PropertyType ?? item.GetType();
            if (drualcman.Helpers.ObjectHelpers.IsGenericList(model.FullName))
            {
                //activate the list found
                Type[] genericType = father.Instance.PropertyType.GetGenericArguments();
                model = genericType[0];
                response.IsList = true;
                response.PropertyListName = father.Instance?.Name ?? model.Name;
                response.PropertyListInstance = item;
            }
            else
            {
                response.IsList = false;
                response.PropertyListName = string.Empty;
                response.PropertyListInstance = null;
            }

            List<Columns> columns = new List<Columns>(Names.HaveColumns(model, response.ActualTable));
            int c = columns.Count;

            if (response.IsList)
            {
                object dat = Activator.CreateInstance(model);
                Model.InstanceProperties(dat);

                for (int index = 0; index < c; index++)
                {
                    if (columns[index].Options is not null)
                    {
                        if (!columns[index].Options.Ignore)
                        {
                            value.SetValue(columns[index].Column.PropertyType.Name, columns[index].Column, dat, reader[columns[index].ColumnName]);
                        }
                    }
                    else
                    {
                        value.SetValue(columns[index].Column.PropertyType.Name, columns[index].Column, dat, reader[columns[index].ColumnName]);
                    }
                }
                response.InUse = dat;
                response.PropertyListData = dat;
            }
            else
            {
                for (int index = 0; index < c; index++)
                {
                    if (columns[index].Options is not null)
                    {
                        if (!columns[index].Options.Ignore)
                        {
                            value.SetValue(columns[index], reader[columns[index].ColumnName]);
                        }
                    }
                    else
                    {
                        value.SetValue(columns[index], reader[columns[index].ColumnName]);
                    }
                }
            }
            return response;
        }
        
    }
}
