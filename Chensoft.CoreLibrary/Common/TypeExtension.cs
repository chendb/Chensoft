

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Chensoft.Common
{
    public static class TypeExtension
    {

        public static bool IsAssignableFrom(this Type type, Type instanceType)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (instanceType == null)
                throw new ArgumentNullException("instanceType");

            if (type.IsGenericType && type.IsGenericTypeDefinition)
            {
                IEnumerable<Type> baseTypes = null;

                if (type.IsInterface)
                {
                    if (instanceType.IsInterface)
                    {
                        baseTypes = new List<Type>(new Type[] { instanceType });
                        ((List<Type>)baseTypes).AddRange(instanceType.GetInterfaces());
                    }
                    else
                    {
                        baseTypes = instanceType.GetInterfaces();
                    }
                }
                else
                {
                    baseTypes = new List<Type>();

                    var currentType = instanceType;

                    while (currentType != typeof(object) &&
                          currentType != typeof(Enum) &&
                          currentType != typeof(Delegate) &&
                          currentType != typeof(ValueType))
                    {
                        ((List<Type>)baseTypes).Add(currentType);
                        currentType = currentType.BaseType;
                    }
                }

                foreach (var baseType in baseTypes)
                {
                    if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == type)
                    {
                        return true;
                    }
                }
            }

            return type.IsAssignableFrom(instanceType);
        }

        public static bool IsImplements(this Type instanceType, Type interfaceType)
        {
            if (instanceType == null || interfaceType == null)
                return false;

            var result = instanceType.FindInterfaces((type, criteria) =>
            {
                if (interfaceType.IsGenericType)
                {
                    if (interfaceType.IsGenericTypeDefinition)
                    {
                        if (type.IsGenericType)
                        {
                            if (type.IsGenericTypeDefinition)
                                return type == interfaceType;
                            else
                                return (type.GetGenericTypeDefinition() == interfaceType);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (type.IsGenericType)
                        {
                            var implementArguments = type.GetGenericArguments();
                            var interfaceArguments = interfaceType.GetGenericArguments();

                            if (implementArguments.Length != interfaceArguments.Length)
                                return false;

                            for (int i = 0; i < implementArguments.Length; i++)
                            {
                                if (!interfaceArguments[i].IsAssignableFrom(implementArguments[i]))
                                    return false;
                            }

                            return true;
                        }
                    }
                }

                return interfaceType.IsAssignableFrom(instanceType);
            }, interfaceType);

            return result != null && result.Length > 0;
        }


        public static bool IsScalarType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsArray)
                return IsScalarType(type.GetElementType());

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsScalarType(type.GetGenericArguments()[0]);

            return type.IsPrimitive || type.IsEnum ||
                   type == typeof(string) || type == typeof(decimal) ||
                   type == typeof(DateTime) || type == typeof(TimeSpan) ||
                   type == typeof(DateTimeOffset) || type == typeof(Guid);
        }

        public static Type GetType(string typeName, bool throwOnError = false, bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            typeName = typeName.Replace(" ", "");

            switch (typeName.ToLowerInvariant())
            {
                case "string":
                    return typeof(string);
                case "string[]":
                    return typeof(string[]);

                case "int":
                    return typeof(int);
                case "int?":
                    return typeof(int?);
                case "int[]":
                    return typeof(int[]);

                case "long":
                    return typeof(long);
                case "long?":
                    return typeof(long?);
                case "long[]":
                    return typeof(long[]);

                case "short":
                    return typeof(short);
                case "short?":
                    return typeof(short?);
                case "short[]":
                    return typeof(short[]);

                case "byte":
                    return typeof(byte);
                case "byte?":
                    return typeof(byte?);
                case "binary":
                case "byte[]":
                    return typeof(byte[]);

                case "bool":
                case "boolean":
                    return typeof(bool);
                case "bool?":
                case "boolean?":
                    return typeof(bool?);
                case "bool[]":
                case "boolean[]":
                    return typeof(bool[]);

                case "money":
                case "currency":
                case "decimal":
                    return typeof(decimal);
                case "money?":
                case "currency?":
                case "decimal?":
                    return typeof(decimal?);
                case "money[]":
                case "currency[]":
                case "decimal[]":
                    return typeof(decimal[]);

                case "float":
                case "single":
                    return typeof(float);
                case "float?":
                case "single?":
                    return typeof(float?);
                case "float[]":
                case "single[]":
                    return typeof(float[]);

                case "double":
                case "number":
                    return typeof(double);
                case "double?":
                case "number?":
                    return typeof(double?);
                case "double[]":
                case "number[]":
                    return typeof(double[]);

                case "uint":
                    return typeof(uint);
                case "uint?":
                    return typeof(uint?);
                case "uint[]":
                    return typeof(uint[]);

                case "ulong":
                    return typeof(ulong);
                case "ulong?":
                    return typeof(ulong?);
                case "ulong[]":
                    return typeof(ulong[]);

                case "ushort":
                    return typeof(ushort);
                case "ushort?":
                    return typeof(ushort?);
                case "ushort[]":
                    return typeof(ushort[]);

                case "sbyte":
                    return typeof(sbyte);
                case "sbyte?":
                    return typeof(sbyte?);
                case "sbyte[]":
                    return typeof(sbyte[]);

                case "char":
                    return typeof(char);
                case "char?":
                    return typeof(char?);
                case "char[]":
                    return typeof(char[]);

                case "date":
                case "time":
                case "datetime":
                    return typeof(DateTime);
                case "date?":
                case "time?":
                case "datetime?":
                    return typeof(DateTime?);
                case "date[]":
                case "time[]":
                case "datetime[]":
                    return typeof(DateTime[]);

                case "timespan":
                    return typeof(TimeSpan);
                case "timespan?":
                    return typeof(TimeSpan?);
                case "timespan[]":
                    return typeof(TimeSpan[]);

                case "guid":
                    return typeof(Guid);
                case "guid?":
                    return typeof(Guid?);
                case "guid[]":
                    return typeof(Guid[]);

                case "object":
                    return typeof(object);
                case "void":
                    return typeof(void);
            }

            if (!typeName.Contains("."))
                typeName = "System." + typeName;

            return Type.GetType(typeName, throwOnError, ignoreCase);
        }

    }
}
