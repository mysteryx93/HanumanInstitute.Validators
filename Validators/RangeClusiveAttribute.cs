using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable MemberCanBePrivate.Global

namespace HanumanInstitute.Validators;

/// <summary>
/// Validates that a property is within range, allowing to exclude the minimum or maximum value. 
/// </summary>
public class RangeClusiveAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets or sets the minimum valid value.
    /// </summary>
    public double? Min { get; set; }

    /// <summary>
    /// Gets or sets whether the minimum value is valid.
    /// </summary>
    public bool MinInclusive { get; set; }

    /// <summary>
    /// Gets or sets the maximum valid value.
    /// </summary>
    public double? Max { get; set; }

    /// <summary>
    /// Gets or sets whether the maximum value is valid.
    /// </summary>
    public bool MaxInclusive { get; set; }

    private string? _lastValidationError;

    /// <summary>
    /// Initializes a new instance of the RangeClusiveAttribute class.
    /// </summary>
    /// <param name="min">The minimum valid value.</param>
    /// <param name="minInclusive">Whether the minimum value is valid.</param>
    /// <param name="max">The maximum valid value.</param>
    /// <param name="maxInclusive">Whether the maximum value is valid.</param>
    public RangeClusiveAttribute(object? min = null, bool minInclusive = true, object? max = null, bool maxInclusive = true)
    {
        Min = min != null ? Convert.ToDouble(min) : null;
        MinInclusive = minInclusive;
        Max = max != null ? Convert.ToDouble(max) : null;
        MaxInclusive = maxInclusive;
        ErrorMessage = @"{0}";
    }
    
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null) { return true; }

        double convertedValue;

        try
        {
            convertedValue = Convert.ToDouble(value);
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            return false;
        }

        _lastValidationError = convertedValue.GetRangeError("{0}", Min, MinInclusive, Max, MaxInclusive);
        return _lastValidationError == null;
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        var error = _lastValidationError?.FormatInvariant(name);
        return ErrorMessageString.FormatInvariant(error);
    }
}
