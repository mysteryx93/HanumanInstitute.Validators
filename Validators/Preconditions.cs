using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable MemberCanBePrivate.Global

namespace HanumanInstitute.Validators;

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
    [return: NotNull]
    [Obsolete("Use Check.NotNull instead for shorter syntax.")]
    public static T CheckNotNull<T>([NotNull, JetBrains.Annotations.NoEnumeration] this T value, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.NotNull(value, name);

    /// <summary>
    /// Validates whether specific value is not null or empty, and throws an exception if it is null or empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.NotNullOrEmpty instead for shorter syntax.")]
    public static string CheckNotNullOrEmpty(string? value, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.NotNullOrEmpty(value, name);

    /// <summary>
    /// Validates whether specific list is not null or empty, and throws an exception if it is null or empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.NotNullOrEmpty instead for shorter syntax.")]
    public static IEnumerable CheckNotNullOrEmpty([NotNull] this IEnumerable? value, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.NotNullOrEmpty(value, name);

    /// <summary>
    /// Validates whether specific list is not null or empty, and throws an exception if it is null or empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.NotNullOrEmpty instead for shorter syntax.")]
    public static IEnumerable<T> CheckNotNullOrEmpty<T>([NotNull, JetBrains.Annotations.NoEnumeration] this IEnumerable<T>? value, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.NotNullOrEmpty(value, name);

    /// <summary>
    /// Validates whether specified type is assignable from specific base class.
    /// </summary>
    /// <param name="value">The Type to validate.</param>
    /// <param name="baseType">The base type that value type must derive from.</param>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.CheckAssignableFrom instead for shorter syntax.")]
    public static Type CheckAssignableFrom(this Type? value, Type baseType, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.AssignableFrom(value, baseType, name);

    /// <summary>
    /// Validates whether specified type derives from specific base class.
    /// </summary>
    /// <param name="value">The Type to validate.</param>
    /// <param name="baseType">The base type that value type must derive from.</param>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.DerivesFrom instead for shorter syntax.")]
    public static Type CheckDerivesFrom(this Type? value, Type baseType, [CallerArgumentExpression(nameof(value))] string name = "") =>
        Check.DerivesFrom(value, baseType, name);

    /// <summary>
    /// Validates whether an enumeration value is valid, since it can contain any integer value.
    /// If the enumeration has FlagsAttribute, it also checks whether value is a combination of valid values.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the property.</param>
    [Obsolete("Use Check.EnumValid instead for shorter syntax.")]
    public static T CheckEnumValid<T>(this T value, [CallerArgumentExpression(nameof(value))] string name = "")
        where T : Enum =>
        Check.EnumValid(value, name);

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
    [Obsolete("Use Check.IsInRange instead.")]
    public static bool IsInRange<T>(this T value, T? min = null, bool minInclusive = true, T? max = null, bool maxInclusive = true)
        where T : struct, IComparable<T> =>
        Check.IsInRange(value, min, minInclusive, max, maxInclusive);

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
    [Obsolete("Use Check.Range instead for shorter syntax.")]
    public static T CheckRange<T>(this T value, [CallerArgumentExpression(nameof(value))] string name = "", T? min = null, bool minInclusive = true, T? max = null,
        bool maxInclusive = true)
        where T : struct, IComparable<T> =>
        Check.Range(value, min, minInclusive, max, maxInclusive, name);

    /// <summary>
    /// Returns the range validation message.
    /// </summary>
    /// <typeparam name="T">The type of data to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="min">The minimum valid value.</param>
    /// <param name="minInclusive">Whether the minimum value is valid.</param>
    /// <param name="max">The maximum valid value.</param>
    /// <param name="maxInclusive">Whether the maximum value is valid.</param>
    /// <returns>The range validation message.</returns>
    [Obsolete("Use Check.GetRangeError instead for shorter syntax.")]
    public static string? GetRangeError<T>(this T value, [CallerArgumentExpression(nameof(value))] string name = "", T? min = null, 
        bool minInclusive = true, T? max = null, bool maxInclusive = true)
        where T : struct, IComparable<T> =>
        Check.GetRangeError(value, min, minInclusive, max, maxInclusive, name);

    /// <summary>
    /// Throws an exception of type ArgumentException saying an argument is null or empty.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    [Obsolete("Use Check.ThrowArgumentNullOrEmpty instead.")]
    public static void ThrowArgumentNullOrEmpty(this string name)
    {
        throw new ArgumentException(Properties.Resources.ValueEmpty.FormatInvariant(name), name);
    }
}
