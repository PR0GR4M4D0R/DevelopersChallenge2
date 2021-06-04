using System;
using System.Globalization;

namespace BankExtract.UI.Web.Utils
{
    /// <summary>
    /// Tools to assist with some conversion.
    /// </summary>
    public static class Tools
    {
        #region " CONSTANTS "

        /// <summary>
        /// Lower limit of a date.
        /// </summary>
        private static DateTime _inferiorLimit = DateTime.Parse("1/1/1753", CultureInfo.InvariantCulture);

        /// <summary>
        /// Upper limit of a date.
        /// </summary>
        private static DateTime _upperLimit = DateTime.Parse("12/31/9999", CultureInfo.InvariantCulture);

        #endregion " CONSTANTS "

        #region " PUBLIC METHODS "

        /// <summary>
        /// Check if the date is valid.
        /// </summary>
        /// <param name="data">Inform date.</param>
        /// <returns>Returns a boolean when the date is valid.</returns>
        public static bool EhDataValida(this DateTime? data)
        {
            return data.HasValue && data >= _inferiorLimit && data <= _upperLimit;
        }

        /// <summary>
        /// Converts an OFX format string to date.
        /// </summary>
        /// <param name="value">OFX string value.</param>
        /// <returns>Returns the converted date.</returns>
        public static DateTime? ConvertStringOFXInDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var year = Convert.ToInt32(value.Substring(0, 4));
            var month = Convert.ToInt32(value.Substring(4, 2));
            var day = Convert.ToInt32(value.Substring(6, 2));
            var hour = Convert.ToInt32(value.Substring(8, 2));
            var minute = Convert.ToInt32(value.Substring(10, 2));
            var second = Convert.ToInt32(value.Substring(12, 2));
            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Converts a string from OFX format to decimal.
        /// </summary>
        /// <param name="value">OFX string value.</param>
        /// <returns>Returns the converted tenth value.</returns>
        public static decimal ConvertStringOFXInDecimal(this string value)
        {
            decimal valorDecimal;
            value = value.Replace('.', ',');
            decimal.TryParse(value, out valorDecimal);
            return valorDecimal;
        }

        #endregion " PUBLIC METHODS "
    }
}