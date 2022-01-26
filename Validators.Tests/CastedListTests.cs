using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xunit;

namespace HanumanInstitute.Validators.Tests;

#pragma warning disable IDE0059 // Unnecessary assignment of a value
public class CastedListTests
{
    [Fact]
    public void CastList_EmptyList_NoException()
    {
        var list = new List<MyBase>();

        var casted = list.CastList<MyDerived, MyBase>();
        foreach (var _ in casted)
        {
        }

        // No exception.
    }

    [Fact]
    public void CastList_SameType_NoException()
    {
        var list = new List<MyBase>
        {
            new MyBase() { Value = 5 }
        };

        var casted = list.CastList<MyBase, MyBase>();
        foreach (var _ in casted)
        {
        }

        // No exception.
    }

    [Fact]
    public void CastList_ListContainsBaseType_ThrowsException()
    {
        var list = new List<MyBase>
        {
            new MyDerived() { Value = "5" },
            new MyBase() { Value = 5 }
        };

        var casted = list.CastList<MyDerived, MyBase>();

        var _ = casted[0];
        Assert.Throws<InvalidCastException>(() => casted[1]);
    }

    [Fact]
    public void CastList_FillListWithDerived_CanReadCastedList()
    {
        var list = new List<MyBase>();

        var casted = list.CastList<MyDerived, MyBase>();
        list.Add(new MyDerived() { Value = "5" });
        list.Add(new MyDerived() { Value = "6" });

        foreach (var _ in casted)
        {
        }
        Assert.Equal("5", casted[0].Value);
        Assert.Equal("6", casted[1].Value);
    }

    [Fact]
    public void CastList_FillListWithDerived_CanReadBaseList()
    {
        var list = new List<MyBase>();

        _ = list.CastList<MyDerived, MyBase>();
        list.Add(new MyDerived() { Value = "5" });
        list.Add(new MyDerived() { Value = "6" });

        foreach (var _ in list)
        {
        }
        Assert.Equal(5, list[0].Value);
        Assert.Equal(6, list[1].Value);
    }

    [Fact]
    public void CastList_FillDerivedList_CanReadBaseList()
    {
        var list = new List<MyBase>();

        var casted = list.CastList<MyDerived, MyBase>();
        casted.Add(new MyDerived() { Value = "5" });
        casted.Add(new MyDerived() { Value = "6" });

        foreach (var _ in list)
        {
        }
        Assert.Equal(5, list[0].Value);
        Assert.Equal(6, list[1].Value);
    }

    [Fact]
    public void CastList_DeleteFromDerived_RemovedFromBase()
    {
        var list = new List<MyBase>();

        var casted = list.CastList<MyDerived, MyBase>();
        casted.Add(new MyDerived() { Value = "5" });
        casted.Add(new MyDerived() { Value = "6" });
        casted.Remove(casted[0]);

        Assert.Single(list);
        Assert.Equal(6, list[0].Value);
    }

    [Fact]
    public void CastList_ToBaseType_CanRead()
    {
        var list = new List<MyDerived>
        {
            new MyDerived() { Value = "5" },
        };

        var casted = list.CastList<MyBase, MyDerived>();

        Assert.Equal(5, casted[0].Value);
    }

    [Fact]
    public void CastList_ObservableCollectionAddBase_CollectionChanged()
    {
        var list = new ObservableCollection<MyDerived>();
        _ = list.CastList<MyBase, MyDerived>();
        var count = 0;
        ((INotifyCollectionChanged)list).CollectionChanged += (_, _) =>
        {
            count++;
        };

        list.Add(new MyDerived());

        Assert.Equal(1, count);
        Assert.Single(list);
    }

    [Fact]
    public void CastList_ObservableCollectionAddCasted_CollectionChanged()
    {
        var list = new ObservableCollection<MyDerived>();
        var casted = list.CastList<MyBase, MyDerived>();
        var count = 0;
        ((INotifyCollectionChanged)list).CollectionChanged += (_, _) =>
        {
            count++;
        };

        casted.Add(new MyDerived());

        Assert.Equal(1, count);
        Assert.Single(list);
    }

    private class MyBase
    {
        public int Value { get; init; }
    }

    private class MyDerived : MyBase
    {
        public new string Value
        {
            get => base.Value.ToStringInvariant();
            init => base.Value = value.Parse<int>() ?? 0;
        }
    }
}
#pragma warning restore IDE0059 // Unnecessary assignment of a value
