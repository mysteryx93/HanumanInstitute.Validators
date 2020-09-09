﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Res = HanumanInstitute.Validators.Properties.Resources;
using System.Collections;

namespace HanumanInstitute.Validators
{
    /// <summary>
    /// Provides helper methods to validate parameters.
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Validates whether specific value is not null, and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        public static T CheckNotNull<T>(this T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether specific value is not null or empty, and throws an exception if it is null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        public static string CheckNotNullOrEmpty(this string value, string name)
        {
            value.CheckNotNull(name);
            if (string.IsNullOrEmpty(value))
            {
                ThrowArgumentNullOrEmpty(name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether specific list is not null or empty, and throws an exception if it is null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        public static IEnumerable CheckNotNullOrEmpty(this IEnumerable value, string name)
        {
            value.CheckNotNull(name);

            if (!value.GetEnumerator().MoveNext())
            {
                ThrowArgumentNullOrEmpty(name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether specific list is not null or empty, and throws an exception if it is null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        public static IEnumerable<T> CheckNotNullOrEmpty<T>(this IEnumerable<T> value, string name)
        {
            value.CheckNotNull(name);
            if (!value.Any())
            {
                ThrowArgumentNullOrEmpty(name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether specified type is assignable from specific base class.
        /// </summary>
        /// <param name="value">The Type to validate.</param>
        /// <param name="baseType">The base type that value type must derive from.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <returns></returns>
        public static Type CheckAssignableFrom(this Type value, Type baseType, string name)
        {
            value.CheckNotNull(name);
            baseType.CheckNotNull(nameof(baseType));

            if (!value.IsAssignableFrom(baseType))
            {
                throw new ArgumentException(Res.TypeMustBeAssignableFromBase.FormatInvariant(name, value.Name, baseType.Name), name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether specified type derives from specific base class.
        /// </summary>
        /// <param name="value">The Type to validate.</param>
        /// <param name="baseType">The base type that value type must derive from.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <returns></returns>
        public static Type CheckDerivesFrom(this Type value, Type baseType, string name)
        {
            value.CheckNotNull(name);
            baseType.CheckNotNull(nameof(baseType));

            if (!value.IsSubclassOf(baseType))
            {
                throw new ArgumentException(Res.TypeMustDeriveFromBase.FormatInvariant(name, value.Name, baseType.Name), name);
            }
            return value;
        }

        /// <summary>
        /// Validates whether an enumeration value is valid, since it can contain any integer value.
        /// If the enumeration has FlagsAttribute, it also checks whether value is a combination of valid values.
        /// </summary>
        /// <typeparam name="T">The type of enumeration.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the property.</param>
        public static T CheckEnumValid<T>(this T value, string name)
            where T : Enum
        {
            var intValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            var defined = Enum.IsDefined(typeof(T), intValue);

            if (!defined && IsEnumTypeFlags<T>())
            {
                defined = CheckEnumValidFlags<T>(intValue);
            }
            if (!defined)
            {
                throw new ArgumentException(Res.ValueInvalidEnum.FormatInvariant(value, name, nameof(T)), name);
            }
            return value;
        }

        private static bool IsEnumTypeFlags<T>()
            where T : Enum
        {
            var attributes =
                typeof(T).GetCustomAttributes(typeof(FlagsAttribute), true);
            return attributes != null && attributes.Length > 0;
        }

        private static bool CheckEnumValidFlags<T>(int value)
            where T : Enum
        {
            var mask = 0;
            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                mask |= (int)enumValue;
            }

            return (mask & value) == value;
        }

        /// <summary>
        /// Returns whether specified value is in valid range.
        /// </summary>
        /// <typeparam name="T">The type of data to validate.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="min">The minimum valid value.</param>
        /// <param name="minInclusive">Whether the minimum value is valid.</param>
        /// <param name="max">The maximum valid value.</param>
        /// <param name="maxInclusive">Whether the maximum value is valid.</param>
        /// <returns>Whether the value is within range.</returns>
        public static bool IsInRange<T>(this T value, T? min = null, bool minInclusive = true, T? max = null, bool maxInclusive = true)
            where T : struct, IComparable<T>
        {
            var minValid = min == null || (minInclusive && value.CompareTo(min.Value) >= 0) || (!minInclusive && value.CompareTo(min.Value) > 0);
            var maxValid = max == null || (maxInclusive && value.CompareTo(max.Value) <= 0) || (!maxInclusive && value.CompareTo(max.Value) < 0);
            return minValid && maxValid;
        }

        /// <summary>
        /// Validates whether specified value is in valid range, and throws an exception if out of range.
        /// </summary>
        /// <typeparam name="T">The type of data to validate.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="min">The minimum valid value.</param>
        /// <param name="minInclusive">Whether the minimum value is valid.</param>
        /// <param name="max">The maximum valid value.</param>
        /// <param name="maxInclusive">Whether the maximum value is valid.</param>
        /// <returns>The value if valid.</returns>
        public static T CheckRange<T>(this T value, string name, T? min = null, bool minInclusive = true, T? max = null, bool maxInclusive = true)
            where T : struct, IComparable<T>
        {
            if (!value.IsInRange(min, minInclusive, max, maxInclusive))
            {
                if (min.HasValue && minInclusive && max.HasValue && maxInclusive)
                {
                    var message = Res.ValueRangeBetween;
                    throw new ArgumentOutOfRangeException(name, value, message.FormatInvariant(name, min, max));
                }
                else
                {
                    var messageMin = min.HasValue ? GetOpText(true, minInclusive).FormatInvariant(min) : null;
                    var messageMax = max.HasValue ? GetOpText(false, maxInclusive).FormatInvariant(max) : null;
                    var message = (messageMin != null && messageMax != null) ?
                        Res.ValueRangeAnd :
                        Res.ValueRange;
                    throw new ArgumentOutOfRangeException(name, value, message.FormatInvariant(name, messageMin ?? messageMax, messageMax));
                }
            }
            return value;
        }

        private static string GetOpText(bool greaterThan, bool inclusive)
        {
            return (greaterThan && inclusive) ? Res.ValueRangeGreaterThanInclusive :
                greaterThan ? Res.ValueRangeGreaterThan :
                inclusive ? Res.ValueRangeLessThanInclusive :
                Res.ValueRangeLessThan;
        }

        /// <summary>
        /// Throws an exception of type ArgumentException saying an argument is null or empty.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public static void ThrowArgumentNullOrEmpty(this string name)
        {
            throw new ArgumentException(Res.ValueEmpty.FormatInvariant(name), name);
        }
    }
}
