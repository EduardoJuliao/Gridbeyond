using System;
using System.Globalization;

namespace GridBeyond.Domain.Extensions
{
    public static class CsvValueExtension
    {
        /// <summary>
        /// Checks if the given string value is a valid record.
        /// </summary>
        /// <param name="value">string value to validate</param>
        /// <param name="date">Date and Time value extracted from string value.</param>
        /// <param name="marketPrice">Market Price extracted from string value.</param>
        /// <returns></returns>
        public static bool IsValid(this string value, out DateTime date, out double marketPrice)
        {
            date = default;
            marketPrice = default;

            var split = value.Split(',');

            if (split.Length != 2)
                return false;

            var formats = new string[] { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy" };

            if (!DateTime.TryParseExact(split[0], formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date))
                return false;

            if (!double.TryParse(split[1], out marketPrice))
                return false;

            return true;
        }
    }
}
