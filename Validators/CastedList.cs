using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HanumanInstitute.Validators
{
    /// <summary>
    /// Represents a casted list that exposes a list as a derived type while maintaining the same references.
    /// </summary>
    /// <typeparam name="TTo">The derived type of the list.</typeparam>
    /// <typeparam name="TFrom">The base type of the list.</typeparam>
    public class CastedList<TTo, TFrom> : IList<TTo>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// Returns the base list that this casted list wraps around.
        /// </summary>
        public IList<TFrom> BaseList { get; }

        public CastedList(IList<TFrom> baseList)
        {
            BaseList = baseList;
            if (baseList is INotifyPropertyChanged prop)
            {
                PropertyChanged += List_PropertyChanged;
            }
            if (baseList is INotifyCollectionChanged coll)
            {
                CollectionChanged += List_CollectionChanged;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void List_PropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

        // IEnumerable
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => BaseList.GetEnumerator();

        // IEnumerable<>
        /// <inheritdoc/>
        public IEnumerator<TTo> GetEnumerator() => new CastedEnumerator<TTo, TFrom>(BaseList.GetEnumerator());

        // ICollection
        /// <inheritdoc/>
        public int Count => BaseList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => BaseList.IsReadOnly;

        /// <inheritdoc/>
        public void Add(TTo item) => BaseList.Add((TFrom)(object)item!);

        /// <inheritdoc/>
        public void Clear() => BaseList.Clear();

        /// <inheritdoc/>
        public bool Contains(TTo item) => BaseList.Contains((TFrom)(object)item!);

        /// <inheritdoc/>
        public void CopyTo(TTo[] array, int arrayIndex) => BaseList.CopyTo((TFrom[])(object)array, arrayIndex);

        /// <inheritdoc/>
        public bool Remove(TTo item) => BaseList.Remove((TFrom)(object)item!);

        // IList
        /// <inheritdoc/>
        public TTo this[int index]
        {
            get { return (TTo)(object)BaseList[index]!; }
            set { BaseList[index] = (TFrom)(object)value!; }
        }

        /// <inheritdoc/>
        public int IndexOf(TTo item) => BaseList.IndexOf((TFrom)(object)item!);

        /// <inheritdoc/>
        public void Insert(int index, TTo item) => BaseList.Insert(index, (TFrom)(object)item!);

        /// <inheritdoc/>
        public void RemoveAt(int index) => BaseList.RemoveAt(index);
    }
}
