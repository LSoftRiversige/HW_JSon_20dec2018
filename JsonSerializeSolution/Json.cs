using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Collections;

namespace JsonSerializeSolution
{
    public static class Json
    {
        public static string ToJson(object obj)
        {
            string result = string.Empty;
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (type.IsEnum)
            {
                result = ValueToString(obj);
            }
            else if (type.Equals(typeof(string)))
            {
                result = AddQuotes(obj);
            }
            else if (obj is IEnumerable array)
            {
                foreach (object element in array)
                {
                    result += ToJson(element) + ",";
                }
                result = $"[{result.TrimEnd(',')}]";
            }
            else if (properties.Length > 0)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    result += PropertyToString(obj, properties[i]) + ",";
                }
                return "{" + result.TrimEnd(',') + "}";
            }
            else if (type.IsValueType)
            {
                result = ValueToString(obj);
            }
            return result;
        }

        private static string PropertyToString(object obj, PropertyInfo p)
        {
            string jsonName = p.Name;
            TryGetNameFromAttribute(p, ref jsonName);
            object value = p.GetValue(obj);
            if (p.PropertyType.IsEnum)
            {
                value = (int)value;
            }
            return $"\"{jsonName}\":{ValueToString(value)}";
        }

        private static bool TryGetNameFromAttribute(PropertyInfo p, ref string jsonName)
        {
            foreach (var a in p.GetCustomAttributes())
            {
                Type type = typeof(MyJsonNameAttribute);
                if (a.GetType() == type)
                {
                    jsonName = (string)type.GetProperty("JSonName").GetValue(a);
                    return true;
                }
            }
            return false;
        }

        private static string AddQuotes(object s)
        {
            return $"\"{s}\"";
        }

        private static string ValueToString(object value)
        {
            string result;
            Type valueType = value.GetType();
            TypeCode typeCode = Type.GetTypeCode(valueType);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    result = value.ToString().ToLower();
                    break;
                case TypeCode.Char:
                    result = AddQuotes(value);
                    break;
                case TypeCode.DateTime:
                    DateTime dt = (DateTime)value;
                    result = AddQuotes(string.Format("{0:s}", dt));
                    break;
                case TypeCode.Decimal:
                    result = NumberToString((decimal)value);
                    break;
                case TypeCode.Double:
                    result = NumberToString(Convert.ToDecimal(value));
                    break;
                case TypeCode.String:
                    result = AddQuotes(value);
                    break;
                case TypeCode.Object:
                    result = ToJson(value);
                    break;
                default:
                    result = value.ToString();
                    break;
            }
            return result;
        }

        private static string NumberToString(decimal value)
        {
            string result = value.ToString(CultureInfo.InvariantCulture);
            if (!result.Contains('.'))
            {
                result += ".0";
            }
            return result;
        }
    }
}
