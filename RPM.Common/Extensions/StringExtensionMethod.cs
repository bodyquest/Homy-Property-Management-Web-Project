namespace RPM.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensionMethod
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string Shortify(this string input, int length)
        {
            if (input == null)
            {
                return input;
            }

            return input.Length > length ? $"{input.Substring(0, length)}..." : input;
        }

        public static string SeparateStringByCapitals(this string str)
        {
            string pattern = @"[A-Z][a-z]+";
            Regex rgx = new Regex(pattern);

            string result = string.Empty;
            MatchCollection matches = rgx.Matches(str);
            foreach (Match match in matches)
            {
                result += match.Value + " ";
            }

            return result.TrimEnd();
        }
    }
}
