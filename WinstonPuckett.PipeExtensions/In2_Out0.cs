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

        /// <summary>
        /// Destructure input tuple, pass it to func, and return a Task.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, Task> asyncFunc)
            => await asyncFunc(input.Item1, input.Item2);

        /// <summary>
        /// Await input tuple, destructure it, and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Action<T, U> func)
            => (await inputTask).Pipe(func);

        /// <summary>
        /// Await input tuple, destructure it, pass it to func, return a Task.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, Task> func)
            => await (await inputTask).PipeAsync(func);

        /// <summary>
        /// Destructure input tuple, and pass it and cancellationToken to func, return a Task.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await asyncFunc(input.Item1, input.Item2, cancellationToken);

        /// <summary>
        /// Await input tuple, destructure it, and pass it and cancellationToken to func, return a Task.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await (await inputTask).PipeAsync(asyncFunc, cancellationToken);
    }
}
