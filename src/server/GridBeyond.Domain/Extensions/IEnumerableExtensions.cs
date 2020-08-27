using System;
using System.Collections.Generic;
using System.Linq;

namespace GridBeyond.Domain.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Get a collection of elements based on condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seq"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> seq, Func<T, T, bool> condition)
        {
            T prev = seq.First();
            List<T> list = new List<T>() { prev };

            foreach (T item in seq.Skip(1))
            {
                if (condition(prev, item) == false)
                {
                    yield return list;
                    list = new List<T>();
                }
                list.Add(item);
                prev = item;
            }

            yield return list;
        }

        /// <summary>
        /// Remove duplicates from IEnumerable.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<string> RemoveDuplicates(this IEnumerable<string> data)
        {
            return data.Distinct();
        }
    }
}
