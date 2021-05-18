using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision.Extensions
{
    public static partial class TypeExtensions
    {
        public static bool IsNullableType(this Type type)
            => type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);


        /// <summary>
        /// Determine if the type is a nullable type.
        /// </summary>
        /// <param name="type">Type being tested.</param>
        public static bool IsNullableType<T>(this T item)
            => item == null || item.GetType().IsNullableType();


        public static bool IsEnumerableType(this Type enumerableType)
            => FindGenericType(typeof(IEnumerable<>), enumerableType) != null;

        /// <summary>
        /// For types like DateTime? this will return DateTime.
        /// </summary>
        /// <param name="type">Type being tested.</param>
        public static Type GetNonNullableType(this Type type)
            => (type.IsNullableType()) ? type.GetGenericArguments()[0] : type;

        /// <summary>
        /// Get Type for underlining structures, will get T of Array[T] or IEnumerable<T> otherwise just return the type passed in.
        /// </summary>
        /// <example></example>
        public static Type GetGenericType(this Type type)
        {
            if (type.IsArray && type.IsEnumerableType())
                return type.GetElementType();

            if (type.IsGenericType && type.IsEnumerableType())
                return type.GetGenericArguments().First();

            return type;
        }

        public static Boolean IsString(this Type type) => (type == typeof(string));

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

        /// <summary>
        /// Returns a more readable friendly name for various types.
        /// </summary>
        /// <param name="type">Type being tested.</param>	    
        public static string GetFriendlyName(this Type type)
        {
            if (type.IsString()) return type.Name;

            if (type.IsEnumerableType())
            {
                return $"{GetFriendlyName(type.GetGenericType())}[]";
            }
            else if (type.IsAnonymousType())
            {
                return $"({String.Join(",", type.GetGenericArguments().Select(GetFriendlyName)) })";
            }
            else if (type.IsGenericType)
            {
                return $"{type.Name.Split('`').First()}<{String.Join(",", type.GetGenericArguments().Select(GetFriendlyName)) }>";
            }
            return type.Name;
        }
    }
}
