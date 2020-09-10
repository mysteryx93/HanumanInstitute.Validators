using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HanumanInstitute.Validators
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns a list of all enumeration flags that are contained in a value.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <param name="value">The value to return the individual flags for.</param>
        /// <returns>A list of single-byte enumeration flags.</returns>
        public static IEnumerable<T> GetFlags<T>(this T value)
            where T : Enum
        {
            var valueLong = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            foreach (var flag in value.GetType().GetEnumValues().Cast<T>())
            {
                if (
                    // enumValue is T flag // cast enumValue to T &&
                    Convert.ToUInt64(flag, CultureInfo.InvariantCulture) is var bitValue // convert flag to ulong
                    && (bitValue & (bitValue - 1)) == 0 // is this a single-bit value?
                    && (valueLong & bitValue) != 0 // is the bit set?
                   )
                {
                    yield return flag;
                }
            }
        }

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
