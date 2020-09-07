using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HanumanInstitute.Validators
{
    public static class ListExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the IList.
        /// </summary>
        /// <typeparam name="T">The type of list items.</typeparam>
        /// <param name="list">The list to add elements to.</param>
        /// <param name="items">The collection whose elements should be added to the end of the IList.</param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            list.CheckNotNull(nameof(list));
            items.CheckNotNull(nameof(items));

            if (list is List<T> castedList)
            {
                castedList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Processes an enumeration in parallel in an awaitable way.
        /// </summary>
        /// <typeparam name="TSource">The type of the source list.</typeparam>
        /// <param name="source">The source list to iterate.</param>
        /// <param name="taskSelector">The operation to evaluate for each item.</param>
        /// <param name="resultProcessor">Callback after each item is evaluated.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        public static async Task ForEachAsync<TSource>(
            this IEnumerable<TSource> source, Func<TSource, Task> taskSelector,
            Action<TSource>? resultProcessor = null, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            taskSelector.CheckNotNull(nameof(taskSelector));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                await Task.WhenAll(
                    from item in source
                    select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime)).ConfigureAwait(false);
            }

            static async Task ProcessAsync(
                TSource item,
                Func<TSource, Task> taskSelector, Action<TSource>? resultProcessor,
                SemaphoreSlim oneAtATime)
            {
                await oneAtATime.WaitAsync().ConfigureAwait(false);
                await taskSelector(item).ConfigureAwait(false);
                try
                {
                    resultProcessor?.Invoke(item);
                }
                finally
                {
                    oneAtATime.Release();
                }
            }
        }

        /// <summary>
        /// Processes an enumeration in parallel in an awaitable way.
        /// </summary>
        /// <typeparam name="TSource">The type of the source list.</typeparam>
        /// <typeparam name="TResult">The result of the operation run on each item.</typeparam>
        /// <param name="source">The source list to iterate.</param>
        /// <param name="taskSelector">The operation to evaluate for each item.</param>
        /// <param name="resultProcessor">Callback after each item is evaluated.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        public static async Task ForEachAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> taskSelector,
            Action<TSource, TResult>? resultProcessor = null, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            taskSelector.CheckNotNull(nameof(taskSelector));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                await Task.WhenAll(
                    from item in source
                    select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime)).ConfigureAwait(false);
            }

            static async Task ProcessAsync(
                TSource item,
                Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult>? resultProcessor,
                SemaphoreSlim oneAtATime)
            {
                await oneAtATime.WaitAsync().ConfigureAwait(false);
                var result = await taskSelector(item).ConfigureAwait(false);
                try
                {
                    resultProcessor?.Invoke(item, result);
                }
                finally
                {
                    oneAtATime.Release();
                }
            }
        }

        /// <summary>
        /// Processes a list in parallel in an awaitable way and returns the output in the same order.
        /// </summary>
        /// <typeparam name="TSource">The type of the source list.</typeparam>
        /// <typeparam name="TResult">The result of the operation run on each item.</typeparam>
        /// <param name="source">The source list to iterate.</param>
        /// <param name="taskSelector">The operation to evaluate for each item.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        /// <returns>The list of results in the same order as source.</returns>
        public static async Task<IList<TResult>> ForEachOrderedAsync<TSource, TResult>(
            this IList<TSource> source, Func<TSource, Task<TResult>> taskSelector, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            taskSelector.CheckNotNull(nameof(taskSelector));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                var indexList = new TResult[source.Count];
                var taskList = new Task[source.Count];
                for (var i = 0; i < source.Count; i++)
                {
                    taskList[i] = ProcessOrderedAsync(source[i], taskSelector, oneAtATime, i, (src, res, index) =>
                    {
                        indexList[index] = res;
                    });
                }
                await Task.WhenAll(taskList).ConfigureAwait(false);

                var result = new List<TResult>(source.Count);
                for (var i = 0; i < source.Count; i++)
                {
                    result.Add(indexList[i]);
                }
                return result;
            }

            static async Task ProcessOrderedAsync(
                TSource item,
                Func<TSource, Task<TResult>> taskSelector,
                SemaphoreSlim oneAtATime, int index, Action<TSource, TResult, int> resultProcessor)
            {
                await oneAtATime.WaitAsync().ConfigureAwait(false);
                var result = await taskSelector(item).ConfigureAwait(false);
                try
                {
                    resultProcessor(item, result, index);
                }
                finally
                {
                    oneAtATime.Release();
                }
            }
        }
    }
}
