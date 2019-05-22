namespace NMyVision.Extensions
{
    public static class NameValueCollectionExtensions
    {

        /// <summary>
        /// Gets and strongly types the values associated with the specified key from the System.Collections.Specialized.NameValueCollection combined into one comma-separated list.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="collection"></param>
        /// <param name="name">The System.String key of the entry that contains the values to get. The key can be null.</param>
        /// <returns>A value of type T that contains a comma-separated list of the values associated with the specified key from the System.Collections.Specialized.NameValueCollection, if found; otherwise, null.</returns>
        /// <example>config.Get("key", 0)</example>
        public static T Get<T>(this System.Collections.Specialized.NameValueCollection collection, string name)
            => Get<T>(collection, name, default(T));


        /// <summary>
        /// Gets and strongly types the values associated with the specified key from the System.Collections.Specialized.NameValueCollection combined into one comma-separated list.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="collection"></param>
        /// <param name="name">The System.String key of the entry that contains the values to get. The key can be null.</param>
        /// <param name="defaultValue">if the specified key is missing return this value.</param>
        /// <returns>A value of type T that contains a comma-separated list of the values associated with the specified key from the System.Collections.Specialized.NameValueCollection, if found; otherwise, null.</returns>
        /// <example>config.Get("key", 0)</example>
        public static T Get<T>(this System.Collections.Specialized.NameValueCollection collection, string name, T defaultValue)
        {
            if (collection[name] == null)
                return defaultValue;

            return collection.Get(name).To<T>();
        }

    }
}
