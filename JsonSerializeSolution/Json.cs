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
            string res = string.Empty;
            Type type = obj.GetType();
            if (type.Equals(typeof(string)))
            {
                res = AddQuotes(obj);
            }
            else
            if (type.IsValueType)
            {
                res = ValueToString(obj);
            }
            else
            if (type.IsArray)
            {
                IEnumerable array = obj as IEnumerable;
                foreach (object element in array)
                {
                    res += ToJson(element)+",";
                }
                res=res.Remove(res.Length-1);
                res = $"[{res}]";
            }
            else
            if (type.IsClass)
            {
                PropertyInfo[] properties = type.GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo p = properties[i];
                    string jsonName = p.Name;
                    TryGetNameFromAttr(p, ref jsonName);
                    res += $"\"{jsonName}\":{ValueToString(p.GetValue(obj))}";
                    int last = properties.Length - 1;
                    if (i < last)
                    {
                        res = $"{res},";
                    }
                }
                return "{" + res + "}";
            }
            return res;
        }

        private static bool TryGetNameFromAttr(PropertyInfo p, ref string jsonName)
        {
            IEnumerable<Attribute> propAttrs = p.GetCustomAttributes();
            foreach (var a in propAttrs)
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
                    result = NumToStr((decimal)value);
                    break;
                case TypeCode.Double:
                    result = NumToStr(Convert.ToDecimal(value));
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

        private static string NumToStr(decimal value)
        {
            string result;
            result = value.ToString(CultureInfo.InvariantCulture);
            if (Math.Truncate(value) == value)
            {
                result += ".0";
            }
            return result;
        }
    }
}
