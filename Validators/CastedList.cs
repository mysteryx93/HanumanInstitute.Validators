using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
// ReSharper disable MemberCanBePrivate.Global

namespace HanumanInstitute.Validators;

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

    /// <summary>
    /// Initializes a new instance of the CastedList class for specified base list.
    /// </summary>
    /// <param name="baseList">The list to cast.</param>
    public CastedList(IList<TFrom> baseList)
    {
        BaseList = baseList;
        if (baseList is INotifyPropertyChanged)
        {
            PropertyChanged += List_PropertyChanged;
        }
        if (baseList is INotifyCollectionChanged)
        {
            CollectionChanged += List_CollectionChanged;
        }
    }

    /// <summary>
    /// Occurs when a property changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// Occurs when the collection changes.
    /// </summary>
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
