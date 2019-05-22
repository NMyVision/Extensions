using System;
using System.Collections.Generic;
using System.Linq;

namespace NMyVision.Extensions
{

    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs the specified action on each element of the System.Collections.Generic.List`1.
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable items</typeparam>
        /// <param name="items">Source to perform action on.</param>
        /// <param name="action">The System.Action`1 delegate to perform on each element of the System.Collections.Generic.List`1.</param>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);

            return items;
        }

        /// <summary>
        /// Performs the specified action on each element of the System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable items</typeparam>
        /// <param name="items">Source to perform action on</param>
        /// <param name="action">The System.Action`1 delegate to perform on each element of the System.Collections.Generic.IEnumerable`1.</param>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in items)
                action(item, index++);

            return items;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable items</typeparam>
        /// <param name="items">Source to perform action on</param>
        /// <param name="action">The System.Action`1 delegate to perform on the System.Collections.Generic.IEnumerable`1.</param>
        /// <returns></returns>
        public static IEnumerable<T> Next<T>(this IEnumerable<T> items, Action<IEnumerable<T>> action)
        {
            action(items);
            return items;
        }

    }
}
