using System.Collections.Generic;

namespace HanumanInstitute.Validators;

/// <summary>
/// Provides extension methods for IDictionary.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key, or <paramref name="defaultValue"/> if the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="defaultValue">The value to return if the key is not found. Defaults to <c>default(TValue)</c>.</param>
    /// <returns>The value associated with the specified key, or <paramref name="defaultValue"/> if the key is not found.</returns>
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue? defaultValue = default!)
    {
        Check.NotNull(dictionary);
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }
}
