using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HanumanInstitute.Validators
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a value to string using InvariantCulture.
        /// </summary>
        /// <typeparam name="T">The type of object to convert.</typeparam>
        /// <param name="value">The object to convert to string.</param>
        /// <returns>The invariant string representation of the object.</returns>
        public static string ToStringInvariant<T>(this T value) => FormattableString.Invariant($"{value}");

        /// <summary>
        /// Returns whether the string contains a value. It is the equivalent of !string.IsNullOrEmpty(value).
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns>Whether the string is not null or empty.</returns>
        public static bool HasValue([NotNullWhen(true)] this string? value) => !string.IsNullOrEmpty(value);

        /// <summary>
        /// Returns specified default value if the value is null or empty.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <param name="defaultValue">The default value if value is null or empty.</param>
        /// <returns>The new value.</returns>
        public static string Default(this string value, string defaultValue) => string.IsNullOrEmpty(value) ? defaultValue : value;

        /// <summary>
        /// Formats a string using invariant culture. This is a shortcut for string.format(CultureInfo.InvariantCulture, ...)
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatInvariant(this string format, params object?[] args) => string.Format(CultureInfo.InvariantCulture, format, args);

        /// <summary>
        /// Parses a string value into specified data type and returns null if conversion fails.
        /// </summary>
        /// <typeparam name="T">The data type to parse into.</typeparam>
        /// <param name="input">The string value to parse.</param>
        /// <returns>The parsed value, or null if parsring failed.</returns>
        public static T? Parse<T>(this string? input) where T : struct
        {
            if (string.IsNullOrEmpty(input)) { return null; }

            try
            {
                var result = TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
                return (T)result;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}
