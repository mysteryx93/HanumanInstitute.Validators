using System;

namespace JetBrains.Annotations;

/// <summary>
/// Tells Rider/ReSharper that the IEnumerable argument is not enumerated.
/// Declared here without Conditional so it is stored in this assembly; consumers do not need JetBrains.Annotations.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class NoEnumerationAttribute : Attribute;
