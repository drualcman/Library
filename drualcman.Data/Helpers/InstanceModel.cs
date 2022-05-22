using System;
using System.Collections.Generic;
using System.Reflection;

namespace drualcman.Data.Helpers
{
    public class InstanceModel
    {
        public void InstanceProperties<TModel>(TModel item)
        {
            PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(PropertyInfo property in properties)
            {
                if(drualcman.Helpers.ObjectHelpers.IsGenericList(property.PropertyType.FullName))
                {
                    Type[] genericType = property.PropertyType.GetGenericArguments();
                    Type creatingCollectionType = typeof(List<>).MakeGenericType(genericType);
                    object dat = Activator.CreateInstance(creatingCollectionType);
                    property.SetValue(item, dat, null);
                }
                else
                {
                    if(property.PropertyType.IsClass && property.PropertyType != typeof(string) && !property.PropertyType.IsArray)
                    {
                        object activation = Activator.CreateInstance(property.PropertyType);
                        property.SetValue(item, activation, null);
                        InstanceProperties(activation);
                    }
                }
            }

        }
    }
}
