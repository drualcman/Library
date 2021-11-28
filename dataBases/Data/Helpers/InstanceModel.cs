using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman.Data.Helpers
{
    internal class InstanceModel
    {
        readonly ReadOnlyCollection<TableName> Tables;

        public InstanceModel(IList<TableName> tables)
        {
            Tables = new ReadOnlyCollection<TableName>(tables);
        }

        public void InstanceProperties<TModel>(TModel item)
        {
            PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                if (drualcman.Helpers.ObjectHelpers.IsGenericList(property.PropertyType.FullName))
                {
                    Type[] genericType = property.PropertyType.GetGenericArguments();
                    Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                    object dat = Activator.CreateInstance(creatingCollectionType);
                    property.SetValue(item, dat, null);
                }
                else
                {
                    TableName table = Tables.FirstOrDefault(i => i.Instance == property);
                    if (table != null)
                    {
                        object activation = Activator.CreateInstance(table.Instance.PropertyType);
                        property.SetValue(item, activation, null);
                        InstanceProperties(activation);
                    }
                    else property.SetValue(item, default, null);
                }
            }

        }
    }
}
