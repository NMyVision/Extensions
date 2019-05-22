using System;
using System.Linq;

namespace NMyVision.Extensions
{

    public static partial class CoreExtensions
    {
        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified type.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="value">An object that implements the System.IConvertible interface.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static T To<T>(this object value)
            => (T)To(value, typeof(T));
        
        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified type.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="value">An object that implements the System.IConvertible interface.</param>
        /// <param name="defaultValue">If conversion fails this value will be returned.</param>
        /// <returns></returns>
        public static T To<T>(this object value, T defaultValue)
        {
            if (value == null) return defaultValue;

            try
            {
                return (T)(To(value, typeof(T)) ?? defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified type.
        /// </summary>
        /// <param name="value">An object that implements the System.IConvertible interface.</param>
        /// <param name="type">The type of object to return..</param>
        /// <returns>An object whose type is conversionType and whose value is equivalent to value. -or- A null reference (Nothing in Visual Basic), if value is null and conversionType is not a value type.</returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static object To(this object value, Type type)
        {

            if (value == null || value == DBNull.Value || type == null)
            {
                return null;
            }

            if (value.GetType() == type) return value;

            type = type.GetNonNullableType();

            try
            {
                return Convert.ChangeType(value, type);
            }
            catch
            {
                var text = value.ToString();

                // Handle -1,1,0 to boolean ...                 

                if (type == typeof(Boolean))
                {
                    if (new string[] { "yes", "1", "-1", "checked", "on", "true" }.Contains(text, StringComparer.OrdinalIgnoreCase))
                        return true;
                    else if (new[] { "no", "0", "off", "false" }.Contains(text, StringComparer.OrdinalIgnoreCase))
                        return false;
                }


                if (type == typeof(DateTime))
                { 
                        // this handles common formats
                        DateTime d;
                        if (DateTime.TryParse(text, out d))
                        {
                            return d;
                        }
                    
                        // uncommon formats
                        var formats = new[] { "yyyyMMdd", "dd/MM/yyyy",  "yyyyMMdd HH:mm",  "dd/MM/yyyy HH:mm", "dd/MM/yyyy HHmm", "MM_dd_yyyy", "ddMMyyyy", "yyyy_MM_dd", "yyyyMMddHHmm", "MMddyy", "yyyyMMddHHmmss"};

                        return DateTime.ParseExact(text, formats, null, System.Globalization.DateTimeStyles.None);
                    
                }

                if (type == typeof(Guid) )                
                    return Guid.Parse(text);

                throw;
            }

        }
    }
}
