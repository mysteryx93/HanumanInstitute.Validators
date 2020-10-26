using System;
using System.Collections;
using System.Collections.Generic;

namespace HanumanInstitute.Validators
{
    /// <summary>
    /// Enumerates through a CastedList.
    /// </summary>
    /// <typeparam name="TTo">The derived type of the list.</typeparam>
    /// <typeparam name="TFrom">The base type of the list.</typeparam>
    public class CastedEnumerator<TTo, TFrom> : IEnumerator<TTo>
    {
        public IEnumerator<TFrom> BaseEnumerator { get; }

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

        // IEnumerator<>
        /// <inheritdoc/>
        public TTo Current { get { return (TTo)(object)BaseEnumerator.Current!; } }


        // IDisposable
        private bool _disposedValue;
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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
