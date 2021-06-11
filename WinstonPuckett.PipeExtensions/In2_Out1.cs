using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class In2_Out1
    {
        /// <summary>
        /// Destructure input tuple, pass it to func, and return the result.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static V Pipe<T, U, V>(this (T, U) input, Func<T, U, V> func)
            => func(input.Item1, input.Item2);

        /// <summary>
        /// Destructure input tuple, pass it to func, and return the result.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task<V> PipeAsync<T, U, V>(this (T, U) input, Func<T, U, Task<V>> asyncFunc)
            => await asyncFunc(input.Item1, input.Item2);

        public static async Task<V> PipeAsync<T, U, V>(this Task<(T, U)> inputTask, Func<T, U, V> func)
            => (await inputTask).Pipe(func);

        public static async Task<V> PipeAsync<T, U, V>(this Task<(T, U)> inputTask, Func<T, U, Task<V>> func)
            => await (await inputTask).PipeAsync(func);

        public static async Task<V> PipeAsync<T, U, V>(this (T, U) input, Func<T, U, CancellationToken, Task<V>> asyncFunc, CancellationToken cancellationToken = default)
            => await input.PipeAsync(i => asyncFunc(i.Item1, i.Item2, cancellationToken));

        public static async Task<V> PipeAsync<T, U, V>(this Task<(T, U)> inputTask, Func<T, U, CancellationToken, Task<V>> asyncFunc, CancellationToken cancellationToken = default)
            => await (await inputTask).PipeAsync(asyncFunc, cancellationToken);

    }
}
