using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
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
        /// Adds the elements of the specified collection to the end of the IList.
        /// </summary>
        /// <param name="list">The list to add elements to.</param>
        /// <param name="items">The collection whose elements should be added to the end of the IList.</param>
        public static void AddRange(this IList list, IEnumerable items)
        {
            list.CheckNotNull(nameof(list));
            items.CheckNotNull(nameof(items));

            if (list is ArrayList castedList)
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
        /// Returns a ReadOnlyCollection wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T">The type of list items.</typeparam>
        /// <param name="list">The list to return as read-only.</param>
        /// <returns>An object that acts as a read-only wrapper around the current IList.</returns>
        public static IList<T> AsReadOnly<T>(this IList<T> list)
        {
            list.CheckNotNull(nameof(list));

            return new ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// Creates a casted list that exposes a list as a derived type while maintaining the same references.
        /// </summary>
        /// <typeparam name="TFrom">The base type of the list.</typeparam>
        /// <typeparam name="TTo">The derived type of the list.</typeparam>
        /// <param name="list">The list to cast.</param>
        /// <returns>A casted list wrapping around the original list.</returns>
        public static IList<TTo> CastList<TTo, TFrom>(this IList<TFrom> list)
            where TTo : TFrom
        {
            list.CheckNotNull(nameof(list));

            return new CastedList<TTo, TFrom>(list);
        }

        /// <summary>
        /// Processes an enumeration in parallel in an awaitable way.
        /// </summary>
        /// <typeparam name="TSource">The type of the source list.</typeparam>
        /// <param name="source">The source list to iterate.</param>
        /// <param name="task">The operation to evaluate for each item.</param>
        /// <param name="callback">Callback after each item is evaluated.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        public static async Task ForEachAsync<TSource>(
            this IEnumerable<TSource> source, Func<TSource, Task> task,
            Action<TSource>? callback = null, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            task.CheckNotNull(nameof(task));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                await Task.WhenAll(
                    from item in source
                    select ProcessAsync(item, task, callback, oneAtATime)).ConfigureAwait(false);
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
        /// <param name="task">The operation to evaluate for each item.</param>
        /// <param name="callback">Callback after each item is evaluated.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        public static async Task ForEachAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> task,
            Action<TSource, TResult>? callback = null, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            task.CheckNotNull(nameof(task));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                await Task.WhenAll(
                    from item in source
                    select ProcessAsync(item, task, callback, oneAtATime)).ConfigureAwait(false);
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
        /// <param name="task">The operation to evaluate for each item.</param>
        /// <param name="maxParallel">The maximum amount of tasks to run in parallel.</param>
        /// <returns>The list of results in the same order as source.</returns>
        public static async Task<IList<TResult>> ForEachOrderedAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> task, int maxParallel = 10)
        {
            source.CheckNotNull(nameof(source));
            task.CheckNotNull(nameof(task));
            maxParallel.CheckRange(nameof(maxParallel), min: 1);

            using (var oneAtATime = new SemaphoreSlim(maxParallel, maxParallel))
            {
                var count = source.Count();
                var indexList = new TResult[count];
                var taskList = new Task[count];
                var i = 0;
                foreach (var item in source)
                {
                    taskList[i] = ProcessOrderedAsync(item, task, oneAtATime, i, (src, res, index) =>
                    {
                        indexList[index] = res;
                    });
                    i++;
                }
                await Task.WhenAll(taskList).ConfigureAwait(false);

                var result = new List<TResult>(count);
                for (i = 0; i < count; i++)
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
