using System;
using System.Collections.Generic;
using System.Linq;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Extensions
{
    public static class DataModelExtensions
    {
        /// <summary>
        /// Return a list of latest dates in sequence with the desired value.
        /// </summary>
        /// <param name="ordered">Ordered data model</param>
        /// <param name="currDate">Lookup Date</param>
        /// <param name="valueToFind">Value to find</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> FindLatestValue(this IOrderedEnumerable<DataModel> ordered, DateTime lookupDate, double valueToFind)
        {
            if (ordered.Count(x => x.MarketPriceEX1 == valueToFind) == 1)
            {
                var index = ordered.ToList().FindIndex(x => x.MarketPriceEX1 == valueToFind);
                yield return ordered.ElementAt(index).Date;
                yield break;
            }

            var indices = ordered
                .Select((data, index) => new { data, index })
                .Where(x => x.data.MarketPriceEX1 == valueToFind && x.data.Date.Date == lookupDate.Date)
                .Select(x => x.index)
                .ToList();

            var result = indices.GroupWhile((x, y) => y - x == 1)
                 .Select(x => new { Count = x.Count(), Elements = x })
                 .ToList();

            var biggestSuccession = result.Last(x => x.Elements.Count() == result.Max(y => y.Count)).Elements;

            foreach (var index in biggestSuccession)
                yield return ordered.ElementAt(index).Date;
        }

        /// <summary>
        /// Return a list of dates in sequence with the desired value.
        /// </summary>
        /// <param name="ordered">Ordered data model</param>
        /// <param name="currDate">Lookup Date</param>
        /// <param name="valueToFind">Value to find</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> FindValue(this IOrderedEnumerable<DataModel> ordered, DateTime lookupDate, double valueToFind)
        {
            if (ordered.Count(x => x.MarketPriceEX1 == valueToFind) == 1)
            {
                yield return ordered.ElementAt(0).Date;
                yield break;
            }

            foreach (var data in ordered.Where(x => x.MarketPriceEX1 == valueToFind && x.Date.Date == lookupDate.Date))
                yield return data.Date;
        }

        /// <summary>
        /// Returns a list of peak times during a day
        /// </summary>
        /// <param name="source"></param>
        /// <param name="currDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> FindLatestPeakDuringDay(this List<DataModel> source, DateTime currDate)
        {
            var maxValue = source.Max(x => x.MarketPriceEX1);
            foreach (var date in source.OrderBy(x => x.Date).FindLatestValue(currDate, maxValue))
                yield return date;
        }

        /// <summary>
        /// Returns a list of quieter times during a day
        /// </summary>
        /// <param name="source"></param>
        /// <param name="currDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> FindLatestQuieterDuringDay(this List<DataModel> source, DateTime currDate)
        {
            var minValue = source.Min(x => x.MarketPriceEX1);
            foreach (var date in source.OrderBy(x => x.Date).FindLatestValue(currDate, minValue))
                yield return date;
        }
    }
}
