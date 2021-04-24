using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace drualcman
{
    public static class Objetos
    {
        /// <summary>
        /// Conocer el tipo del objeto enviado
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string GetTipo(object sender)
        {
                return sender.GetType().Name;

        }

        /// <summary>
        /// Get a value from a porperty on the object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Object GetPropValue(this Object obj, String name)
        {
            if (obj == null) return null;
            else
            {
                // Split property name to parts (propertyName could be hierarchical, like obj.subobj.subobj.property
                string[] propertyNameParts = name.Split('.');

                foreach (String part in propertyNameParts)
                {
                    Type type = obj.GetType();
                    PropertyInfo info = type.GetProperty(part);
                    if (info == null) return null;
                    else obj = info.GetValue(obj, null);
                }
                return obj;
            }
        }

        /// <summary>
        /// Get a value from the object on the specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetPropValue<T>(this Object obj, String name)
        {
            // throws InvalidCastException if types are incompatible
            Object retval = GetPropValue(obj, name);
            if (retval == null) return default(T);
            else return (T)retval;
        }
    }
}
