using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetDictionaryType(this Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            {
                return type;
            }
            return (from t in type.GetInterfaces()
                    where t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    select t).FirstOrDefault<Type>();
        }

        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public static bool IsCollectionType(this Type type)
        {
            return ((type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ICollection<>))) ||
                    (from t in type.GetInterfaces()
                     where t.IsGenericType
                     select t.GetGenericTypeDefinition()).Any<Type>(t => (t == typeof(ICollection<>))));
        }

        public static bool IsDictionaryType(this Type type)
        {
            return ((type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))) ||
                    (from t in type.GetInterfaces()
                     where t.IsGenericType
                     select t.GetGenericTypeDefinition()).Any<Type>(t => (t == typeof(IDictionary<,>))));
        }

        public static bool IsEnumerableType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        public static bool IsListOrDictionaryType(this Type type)
        {
            if (!type.IsListType())
            {
                return type.IsDictionaryType();
            }
            return true;
        }

        public static bool IsListType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IList));
        }

        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static string ToNullSafeString(this object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return null;
        }
    }
}
