using System.Collections;
using System.Collections.Generic;
// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
namespace System;

/// <summary>
/// Enumerates through a CastedList.
/// </summary>
/// <typeparam name="TTo">The derived type of the list.</typeparam>
/// <typeparam name="TFrom">The base type of the list.</typeparam>
public class CastedEnumerator<TTo, TFrom> : IEnumerator<TTo>
{
    /// <summary>
    /// The IEnumerable passed to the constructor.
    /// </summary>
    public IEnumerator<TFrom> BaseEnumerator { get; }

    /// <summary>
    /// Initializes a new instance of the CastedEnumerator class for specified base enumerator.
    /// </summary>
    /// <param name="baseEnumerator">The enumerator to cast.</param>
    public CastedEnumerator(IEnumerator<TFrom> baseEnumerator)
    {
        BaseEnumerator = baseEnumerator;
    }

    // IEnumerator
    /// <inheritdoc/>
    object IEnumerator.Current { get { return BaseEnumerator.Current!; } }
    /// <inheritdoc/>
    public bool MoveNext() { return BaseEnumerator.MoveNext(); }
    /// <inheritdoc/>
    public void Reset() { BaseEnumerator.Reset(); }

    /// <inheritdoc/>
    public TTo Current { get { return (TTo)(object)BaseEnumerator.Current!; } }


    // IDisposable
    private bool _disposedValue;
    /// <summary>
    /// Dispose of all resources.
    /// </summary>
    /// <param name="disposing">True if called from Dispose, false if called from a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                BaseEnumerator.Dispose();
            }
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Dispose all resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
