using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dfe.Wizard.Prototype.Models.Common
{
    public static class Extensions
    {
        public static string Encode(this string unencoded)
        {
            var replacementChars = new Dictionary<string, string> {{"Ã§","ç"}};
            var newString = string.Empty;
            foreach(var c in replacementChars)
            {
                newString = unencoded.Replace(c.Key, c.Value);
            }

            return newString;
        }

        public static DateTime ToDateTimeWhenSureNotNull(this string potentialDateString)
        {
            var date = potentialDateString.ToDateTimeOrNull();
            if (date.HasValue)
                return date.Value;

            return DateTime.MinValue;
        }

        public static int ToIntOrZero(this string potentialNumber)
        {
            var number = 0;
            if (int.TryParse(potentialNumber, out number))
            {
                return number;
            }

            return 0;
        }

        public static DateTime? ToDateTimeOrNull(this string potentialDateString)
        {
            if (DateTime.TryParseExact(potentialDateString, "d-M-yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out var newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "dd-MM-yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "d/M/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            if (DateTime.TryParseExact(potentialDateString, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out newDate))
            {
                return newDate;
            }

            return null;
        }
    }
}
