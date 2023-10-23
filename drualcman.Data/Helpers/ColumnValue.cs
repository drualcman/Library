using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace drualcman.Data.Helpers
{
    public class ColumnValue
    {
        readonly ReadOnlyCollection<TableName> Tables;
        readonly object Item;

        public ColumnValue(IList<TableName> tables, object item)
        {
            Tables = new ReadOnlyCollection<TableName>(tables);
            Item = item;
        }

        public void SetValue(Columns column, DataRow row)
        {
            SetValue(column, row[column.ColumnName]);
        }

        public void SetValue(Columns column, object row)
        {
            try
            {
                if(column.TableIndex > 0)
                {
                    SetValue(column.PropertyType, column.Column, Item.GetPropValue(Tables[column.TableIndex].Instance.Name), row);
                }
                else
                {
                    SetValue(column.PropertyType, column.Column, Item, row);
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Console.WriteLine("ex 2 {0}, column: {1}, value: {2}", err, column.ColumnName, row);
                SetValue(column.Column, Item, row);
            }

        }

        private void SetValue(PropertyInfo sender, object destination, object value)
        {
            try
            {
                if(value != null && value.GetType() != typeof(DBNull))
                {
                    sender.SetValue(destination, Convert.ChangeType(value, Nullable.GetUnderlyingType(sender.PropertyType)));
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Console.WriteLine("ex 3 {0}, sender: {1}, value: {2}, destination: {3}", err, sender.Name, value, destination);
                Console.WriteLine("ex 3 {0}", err);
            }

        }

        public void SetValue(string propertyType, PropertyInfo sender, object destination, object value)
        {
            if(value.GetType() != typeof(DBNull))
            {
                switch(propertyType)
                {
                    case "bool":
                        if(int.TryParse(value.ToString(), out int test))
                        {
                            if(test == 0) sender.SetValue(destination, false);
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
                        sender.SetValue(destination, ConvertIntoDate(value));
                        break;
                    case "nullable":
                        sender.SetValue(destination, value);
                        break;
                    default:
                        if(sender.PropertyType.Equals(value.GetType())) sender.SetValue(destination, value);
                        else
                        {
                            if(sender.PropertyType.BaseType == typeof(Enum))
                            {
                                if(int.TryParse(value.ToString(), out int index))
                                {
                                    sender.SetValue(destination, index);
                                }
                                else
                                {
                                    sender.SetValue(destination, Enum.Parse(sender.PropertyType, value.ToString()));
                                }
                            }
                            else if(sender.PropertyType == typeof(DateTime)) sender.SetValue(destination, ConvertIntoDate(value));
                            else if(sender.PropertyType == typeof(String)) sender.SetValue(destination, value.ToString());
                            else if(value.GetType().IsAssignableTo(typeof(IConvertible))) sender.SetValue(destination, Convert.ChangeType(value, sender.PropertyType));
                            else sender.SetValue(destination, value);
                        }
                        break;
                }
            }
        }

        private DateTime ConvertIntoDate(object value)
        {
            DateTime date;
            try
            {
                date = Convert.ToDateTime(value);
            }
            catch(Exception ex)
            {
                string err = ex.Message;
                Console.WriteLine("ex ConvertIntoDate {0}", err);

                if(!DateTime.TryParse(value.ToString(), out date))
                {
                    if(!TimeSpan.TryParse(value.ToString(), out TimeSpan time))
                    {
                        time = new TimeSpan(DateTime.Now.Ticks);
                    }
                    date = new DateTime(time.Ticks);
                }
            }
            return date;
        }
    }
}
