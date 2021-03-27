using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Dfe.Wizard.Prototype.Models.Common;

namespace Dfe.Wizard.Prototype.Models.Questions
{
    public class ValidationRules
    {
        // TODO add regexes
        public static IDictionary<ValidatorType, Func<string, bool>> GetValidationRules(string compareValue)
        {
            return new Dictionary<ValidatorType, Func<string, bool>>
            {
                {ValidatorType.AlphabeticalValues, s => RegexMatch(s, @"[a-zA-Z]")},
                {ValidatorType.AlphabeticalIncludingSpecialChars, s => RegexMatch(s, @"^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")},
                {ValidatorType.DateTimeHistorical, givenValue => DateMatch(givenValue, g => g < DateTime.Today ) },
                {ValidatorType.DateTimeFuture, givenValue => DateMatch(givenValue, g => g > DateTime.Today )},
                {ValidatorType.Regex, givenValue => RegexMatch(givenValue, compareValue)},
                {ValidatorType.Number, givenValue => NumberMatch(givenValue) },
                {ValidatorType.NumberHigher, givenValue => NumberMatch(givenValue, compareValue, (g, c) => g > c)},
                {ValidatorType.NumberHigherOrEqual, givenValue => NumberMatch(givenValue, compareValue,(g, c) => g >= c)},
                {ValidatorType.NumberLower, givenValue => NumberMatch(givenValue, compareValue,(g, c) => g < c)},
                {ValidatorType.NumberLowerOrEqual, givenValue => NumberMatch(givenValue, compareValue,(g, c) => g <= c)},
                {ValidatorType.LAESTABNumber, givenValue => RegexMatch(givenValue, compareValue)},
                {ValidatorType.MaxCharacters, givenValue => givenValue.Length < GetNumber(compareValue)},
                {ValidatorType.None, givenValue => true}
            };
        }

        private static bool RegexMatch(string value, string pattern)
        {
            var match = Regex.Match(value, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }

        private static int GetNumber(string givenValue)
        {
            if(int.TryParse(givenValue, out var newNumber))
            {
                return newNumber;
            }

            throw new InvalidDataException("The validation failed due to the compare givenValue being null");
        }

        private static bool NumberMatch(string givenValue, string compareValue=null, Func<int, int, bool> check=null)
        {
            if(int.TryParse(givenValue, out var newNumber))
            {
                if (check == null)
                {
                    return true;
                }

                if (check(newNumber, GetNumber(compareValue)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool DateMatch(string givenValue, Func<DateTime, bool> check=null)
        {
            var newDate = givenValue.ToDateTimeOrNull();
            if(newDate != null)
            {
                if (check == null)
                {
                    return true;
                }

                if (check(newDate.Value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
