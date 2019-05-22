using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision.Extensions
{
    public static partial class TypeExtensions
    {
        public static bool IsNullableType( this Type type)
            => type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);


        /// <summary>
        /// Determine if the type is a nullable type.
        /// </summary>
        /// <param name="type">Type being tested.</param>
        public static bool IsNullableType<T>(this T item)
            => item == null || item.GetType().IsNullableType();
        

        public static bool IsEnumerableType(this Type enumerableType)
            => FindGenericType(typeof(IEnumerable<>), enumerableType) != null;

        public static Type GetNonNullableType(this Type type)        
            => (type.IsNullableType()) ? type.GetGenericArguments()[0] : type;

        /// <summary>
        /// Determine if the Type is an anonymous object.
        /// </summary>
        /// <param name="type">Type being tested.</param>
        public static Boolean IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(
                typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                false).Any();
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");

            return hasCompilerGeneratedAttribute && nameContainsAnonymousType;
        }

        internal static Type FindGenericType(Type definition, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
                    return type;
                if (definition.IsInterface)
                {
                    foreach (Type itype in type.GetInterfaces())
                    {
                        Type found = FindGenericType(definition, itype);
                        if (found != null)
                            return found;
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        public static bool IsSimpleType(this Type type)
        {
            type = type.GetNonNullableType();

            return (type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(DateTime))
              || type.Equals(typeof(Guid))
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal)));
        }
    }
}
