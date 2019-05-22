using System;
using System.Data;

namespace NMyVision.Extensions
{
    public static class SqlExtensions
    {


        //Get Ordinal will throw exception if column is missing
        [System.Diagnostics.DebuggerStepThrough]
        static int GetIndex(IDataReader reader, string name)
        {
            try
            {
                return reader.GetOrdinal(name);
            }
            catch
            {
                return -1;
            }
        }

        #region Get Methods

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an System.String.</returns>
        public static string Get(this IDataReader reader, string name)
        {
            var index = GetIndex(reader, name);
            if (index == -1)
            {
                throw new IndexOutOfRangeException($"{name} not found in results. [SqlReader.Get(name)]");
            }
            return Get<string>(reader, index);
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <typeparam name="T">Strongly typed result.</typeparam>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as T.</returns>
        public static T Get<T>(this IDataReader reader, string name)
        {
            var index = GetIndex(reader, name);
            if (index == -1)
            {
                throw new IndexOutOfRangeException($"{name} not found in results. [SqlReader.Get<T>(name)]");
            }

            try
            {
                return Get<T>(reader, index);
            }
            catch (InvalidCastException ex)
            {
                ex.Source = $"column: { name }";
                throw ex;
            }
        }

        /// <summary>
        /// Gets the column with the specified name. If value cannot be converted defaultValue or is null is returned.
        /// </summary>
        /// <typeparam name="T">Strongly typed result.</typeparam>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The column with the specified name as T.</returns>
        public static T Get<T>(this IDataReader reader, string name, T defaultValue)
        {
            var index = GetIndex(reader, name);

            return Get<T>(reader, index, defaultValue);
        }


        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <returns>The column with the specified name as an System.String.</returns>
        public static string Get(this IDataReader reader, int index)
        {
            return Get<string>(reader, index);
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <returns>The column with the specified name as T.</returns>
        public static T Get<T>(this IDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                if (typeof(T) == typeof(string))
                {
                    return (null as string).To<T>();
                }
                else if (!(typeof(T).IsNullableType()))
                {
                    throw new NullReferenceException($"Null was returned for index: {index} and type { typeof(T).Name } is not nullable. [SqlReader.Get<T>(index)]");
                }
            }

            return reader[index].To<T>();
        }

        /// <summary>
        /// Gets the column with the specified name. If value cannot be converted defaultValue or is null is returned.
        /// </summary>
        /// <typeparam name="T">Strongly typed result.</typeparam>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The column located at the specified index as an type but returned as T.</returns>
        public static T Get<T>(this IDataReader reader, int index, T defaultValue)
        {
            if (index == -1) return defaultValue;

            var type = typeof(T);

            if (reader.IsDBNull(index) && type.IsNullableType())
            {
                return defaultValue;
            }

            return reader[index].To<T>(defaultValue);
        }

        /// <summary>
        /// Gets the column with the specified name. If value cannot be converted defaultValue or is null is returned.
        /// </summary>
        /// <typeparam name="T">Strongly typed result.</typeparam>
        /// <param name="reader">Source IDataReader</param>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <param name="type">The type the column result will be converted to.</param>
        /// <returns>The column located at the specified index as an type but returned as System.Object.</returns>
        public static object Get(this IDataReader reader, int index, Type type)
        {
            try
            {
                return reader[index].To(type);
            }
            catch (InvalidCastException ex)
            {
                ex.Source = $"index: { index }";
                throw ex;
            }
        }
        #endregion

    }
}
