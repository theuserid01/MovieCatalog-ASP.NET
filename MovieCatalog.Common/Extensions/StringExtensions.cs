namespace MovieCatalog.Common.Extensions
{
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string CutText(this string text, int maxLen)
        {
            if (text == null || text.Length <= maxLen)
            {
                return text;
            }

            return text.Substring(0, maxLen) + "...";
        }

        public static DateTime ParseToDateTime(this string text)
        {
            string[] formats = new string[]
            {
                "MM/dd/yyyy",
                "d MMM yyyy",
                "d MMMM yyyy",
                "MMM dd yyyy",
                "MMM dd, yyyy"
            };

            if (DateTime.TryParseExact(text, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateTime dateTime))
            {
                return dateTime;
            }

            return new DateTime(1900, 1, 1);
        }

        public static decimal ParseToDecimal(this string text)
        {
            if (decimal.TryParse(text, out decimal number))
            {
                return number;
            }

            return 0;
        }

        public static double ParseToDouble(this string text)
        {
            if (double.TryParse(text, out double number))
            {
                return number;
            }

            return 0;
        }

        public static int ParseToInt(this string text)
        {
            if (int.TryParse(text, out int number))
            {
                return number;
            }

            return 0;
        }

        public static string RemoveExtraWhitespace(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Regex.Replace(text, @"\s+", " ");
            }

            return text;
        }
    }
}
