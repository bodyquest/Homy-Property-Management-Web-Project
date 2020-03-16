namespace RPM.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

            return input.Length > length ? $"{input.Substring(length)}..." : input;
        }
    }
}
