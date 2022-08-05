using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HanumanInstitute.Validators;

/// <summary>
/// Provide misc extension methods. 
/// </summary>
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
    /// Forces a value to be within specified range.
    /// </summary>
    /// <typeparam name="T">The type of value to clamp.</typeparam>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The lowest value that can be returned.</param>
    /// <param name="max">The highest value that can be returned.</param>
    /// <returns>The clamped value.</returns>
    public static T Clamp<T>(this T value, T min, T max) 
        where T : IComparable<T>
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

    /// <summary>
    /// Returns whether given type is assignable from specified generic base type.
    /// </summary>
    /// <param name="givenType">The type to validate.</param>
    /// <param name="genericType">The generic base type to check against.</param>
    /// <returns>True if givenType can be converted to genericType, otherwise False.</returns>
    public static bool IsAssignableFromGeneric(this Type givenType, Type genericType)
    {
        givenType.CheckNotNull(nameof(givenType));
        genericType.CheckNotNull(nameof(genericType));

        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        if (baseType == null)
        {
            return false;
        }

        return IsAssignableFromGeneric(baseType, genericType);
    }
}
