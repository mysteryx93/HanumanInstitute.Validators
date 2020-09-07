using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HanumanInstitute.Validators
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Copies all fields from one instance of a class to another.
        /// </summary>
        /// <typeparam name="T">The type of class to copy.</typeparam>
        /// <param name="source">The class to copy.</param>
        /// <param name="target">The class to copy to.</param>
        public static void CopyAllFields<T>(T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                if (targetProperty?.SetMethod != null)
                {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                }
            }
            foreach (var sourceField in type.GetFields())
            {
                var targetField = type.GetField(sourceField.Name);
                targetField?.SetValue(target, sourceField.GetValue(source));
            }
        }


        /// <summary>
        /// Parses a string value into specified data type and returns null if conversion fails.
        /// </summary>
        /// <typeparam name="T">The data type to parse into.</typeparam>
        /// <param name="input">The string value to parse.</param>
        /// <returns>The parsed value, or null if parsring failed.</returns>
        public static Nullable<T> Parse<T>(this string input) where T : struct
        {
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

        /// <summary>
        /// Forces a value to be within specified range.
        /// </summary>
        /// <typeparam name="T">The type of value to clamp.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The lowest value that can be returned.</param>
        /// <param name="max">The highest value that can be returned.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            else if (value.CompareTo(max) > 0)
            {
                return max;
            }
            return value;
        }
    }
}
