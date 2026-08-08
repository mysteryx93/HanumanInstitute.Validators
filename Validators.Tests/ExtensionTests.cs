using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HanumanInstitute.Validators.Tests;

public class ExtensionTests
{
    private static IList<int> CreateList(int length)
    {
        var result = new List<int>(length);
        for (var i = 0; i < length; i++)
        {
            result.Add(i);
        }
        return result;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task ForEachAsync_List_CallbackCalledForEachItem(int length)
    {
        var source = CreateList(length);
        var count = 0;

        await source.ForEachAsync(x => Task.FromResult(x.ToStringInvariant()), (_, _) =>
        {
            count++;
        });

        Assert.Equal(length, count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task ForEachOrderedAsync_TaskFromResult_ResultContainsOrderedItems(int length)
    {
        var source = CreateList(length);

        var result = await source.ForEachOrderedAsync(x => Task.FromResult(x.ToStringInvariant()));

        Assert.Equal(length, result.Count);
        for (var i = 0; i < length; i++)
        {
            Assert.Equal(i.ToStringInvariant(), result[i]);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task ForEachOrderedAsync_TaskRun_ResultContainsOrderedItems(int length)
    {
        var source = CreateList(length);

        var result = await source.ForEachOrderedAsync(x => Task.Run(() => x.ToStringInvariant()));

        Assert.Equal(length, result.Count);
        for (var i = 0; i < length; i++)
        {
            Assert.Equal(i.ToStringInvariant(), result[i]);
        }
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(5.55555)]
    [InlineData(-5.55555)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    public void Parse_Double_ParseValue(double value)
    {
        var str = value.ToStringInvariant();

        var result = str.Parse<double>();

        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(5.55555)]
    [InlineData(-5.55555)]
    public void Parse_Decimal_ParseValue(decimal value)
    {
        var str = value.ToStringInvariant();

        var result = str.Parse<decimal>();

        Assert.Equal(value, result);
    }
    
    [Theory]
    [InlineData(TestEnum.Value1)]
    [InlineData(TestEnum.Value3)]
    public void Parse_Enum_ParseValue(TestEnum value)
    {
        var str = value.ToStringInvariant();

        var result = str.Parse<TestEnum>();

        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("--5")]
    public void Parse_InvalidInt_ReturnsNull(string value)
    {
        var result = value.Parse<int>();

        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("--5")]
    public void Parse_InvalidDouble_ReturnsNull(string value)
    {
        var result = value.Parse<double>();

        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("--5")]
    public void Parse_InvalidDecimal_ReturnsNull(string value)
    {
        var result = value.Parse<decimal>();

        Assert.Null(result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    public void Parse_InvalidEnum_ParseValue(string value)
    {
        var str = value.ToStringInvariant();

        var result = str.Parse<TestEnum>();

        Assert.Null(result);
    }

    [Theory]
    [InlineData("HelloWorld", "hello", true)]
    [InlineData("HelloWorld", "HELLO", true)]
    [InlineData("HelloWorld", "world", false)]
    [InlineData("HelloWorld", "xyz", false)]
    [InlineData("HelloWorld", "", true)]
    [InlineData(null, "a", false)]
    [InlineData("", "a", false)]
    [InlineData("", "", true)]
    public void StartsWithInvariant_Various_ReturnsExpected(string value, string value2, bool expected)
    {
        var result = value.StartsWithInvariant(value2);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("HelloWorld", "world", true)]
    [InlineData("HelloWorld", "WORLD", true)]
    [InlineData("HelloWorld", "hello", false)]
    [InlineData("HelloWorld", "xyz", false)]
    [InlineData("HelloWorld", "", true)]
    [InlineData(null, "a", false)]
    [InlineData("", "a", false)]
    [InlineData("", "", true)]
    public void EndsWithInvariant_Various_ReturnsExpected(string value, string value2, bool expected)
    {
        var result = value.EndsWithInvariant(value2);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("HelloWorld", "lowo", true)]
    [InlineData("HelloWorld", "LOWO", true)]
    [InlineData("HelloWorld", "hello", true)]
    [InlineData("HelloWorld", "world", true)]
    [InlineData("HelloWorld", "xyz", false)]
    [InlineData("HelloWorld", "", true)]
    [InlineData(null, "a", false)]
    [InlineData("", "a", false)]
    [InlineData("", "", true)]
    public void ContainsInvariant_Various_ReturnsExpected(string value, string value2, bool expected)
    {
        var result = value.ContainsInvariant(value2);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetValueOrDefault_ClassKeyExists_ReturnsValue()
    {
        IDictionary<string, string> dictionary = new Dictionary<string, string> { ["a"] = "value" };

        var result = dictionary.GetValueOrDefault("a");

        Assert.Equal("value", result);
    }

    [Fact]
    public void GetValueOrDefault_ClassKeyMissing_ReturnsDefaultNull()
    {
        IDictionary<string, string> dictionary = new Dictionary<string, string> { ["a"] = "value" };

        var result = dictionary.GetValueOrDefault("missing");

        Assert.Null(result);
    }

    [Fact]
    public void GetValueOrDefault_ClassKeyMissing_ReturnsProvidedDefault()
    {
        IDictionary<string, string> dictionary = new Dictionary<string, string> { ["a"] = "value" };

        var result = dictionary.GetValueOrDefault("missing", "n/a");

        Assert.Equal("n/a", result);
    }

    [Fact]
    public void GetValueOrDefault_ClassStoredNull_ReturnsNull()
    {
        IDictionary<string, string> dictionary = new Dictionary<string, string> { ["a"] = null };

        var result = dictionary.GetValueOrDefault("a");

        Assert.Null(result);
    }

    [Fact]
    public void GetValueOrDefault_StructKeyExists_ReturnsValue()
    {
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["a"] = 42 };

        var result = dictionary.GetValueOrDefault("a");

        Assert.Equal(42, result);
    }

    [Fact]
    public void GetValueOrDefault_StructKeyMissing_ReturnsDefaultZero()
    {
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["a"] = 42 };

        var result = dictionary.GetValueOrDefault("missing");

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetValueOrDefault_StructKeyMissing_ReturnsProvidedDefault()
    {
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["a"] = 42 };

        var result = dictionary.GetValueOrDefault("missing", -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void GetValueOrDefault_StructZeroValue_ReturnsZero()
    {
        IDictionary<string, int> dictionary = new Dictionary<string, int> { ["a"] = 0 };

        var result = dictionary.GetValueOrDefault("a");

        Assert.Equal(0, result);
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
}
