using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HanumanInstitute.Validators.Tests
{
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

            await source.ForEachAsync(x => Task.FromResult(x.ToStringInvariant()), (item, result) =>
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
        public async Task ForEachOrderedAsync_TaskFromResult_ResultContainsOrdererdItems(int length)
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
        public async Task ForEachOrderedAsync_TaskRun_ResultContainsOrdererdItems(int length)
        {
            var source = CreateList(length);

            var result = await source.ForEachOrderedAsync(x => Task.Run<string>(() => x.ToStringInvariant()));

            Assert.Equal(length, result.Count);
            for (var i = 0; i < length; i++)
            {
                Assert.Equal(i.ToStringInvariant(), result[i]);
            }
        }
    }
}
