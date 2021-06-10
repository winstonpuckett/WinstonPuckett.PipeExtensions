using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class In2_Out0
    {
        /// <summary>
        /// Destructure input tuple and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static void Pipe<T, U>(this (T, U) input, Action<T, U> func)
            => func(input.Item1, input.Item2);

        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, Task> asyncFunc)
            => await asyncFunc(input.Item1, input.Item2);

        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Action<T, U> func)
            => (await inputTask).Pipe(func);

        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, Task> func)
            => await (await inputTask).PipeAsync(func);

        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await input.PipeAsync(i => asyncFunc(i.Item1, i.Item2, cancellationToken));

        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await (await inputTask).PipeAsync(asyncFunc, cancellationToken);
    }
}
