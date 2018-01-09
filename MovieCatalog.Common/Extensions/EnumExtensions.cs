namespace MovieCatalog.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string fromString)
             where T : struct, IConvertible, IFormattable
        {
            Enum.TryParse(fromString, true, out T outEnum);

            return outEnum;
        }

        public static T ToEnum<T>(IEnumerable<string> fromListOfString)
             where T : struct, IConvertible, IFormattable
        {
            IEnumerable<T> enumList = fromListOfString.Select(x =>
            {
                Enum.TryParse(x, true, out T outEnum);

                return outEnum;
            });

            return ToEnum(enumList);
        }

        public static T ToEnum<T>(IEnumerable<T> fromListOfEnums)
            where T : struct, IConvertible, IFormattable
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            IEnumerable<int> intlist = fromListOfEnums.Select(x => x.ToInt32(provider));
            int aggregatedint = intlist.Aggregate((prev, next) => prev | next);

            return (T)Enum.ToObject(EnumType<T>(), aggregatedint);
        }

        public static IEnumerable<T> ToFlagsList<T>(T fromSingleEnum)
             where T : struct, IConvertible, IFormattable
        {
            return fromSingleEnum.ToString()
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(strEnum =>
                {
                    Enum.TryParse(strEnum, true, out T outEnum);

                    return outEnum;
                });
        }

        public static IEnumerable<T> ToFlagsList<T>(IEnumerable<string> fromStringEnumList)
             where T : struct, IConvertible, IFormattable
        {
            return fromStringEnumList
                .Select(strEnum =>
                {
                    Enum.TryParse(strEnum, true, out T outEnum);

                    return outEnum;
                });
        }

        public static string ToString<T>(this T fromEnum)
             where T : struct, IConvertible, IFormattable
        {
            return fromEnum.ToString();
        }

        public static string ToString<T>(this IEnumerable<T> fromFlagsList)
             where T : struct, IConvertible, IFormattable
        {
            return ToString(ToEnum(fromFlagsList));
        }

        public static object ToUnderlyingType<T>(T fromeEnum)
             where T : struct, IConvertible, IFormattable
        {
            return Convert.ChangeType(fromeEnum, Enum.GetUnderlyingType(EnumType<T>()));
        }

        private static Type EnumType<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Provided type must be an enum");
            }

            return typeof(T);
        }
    }
}
