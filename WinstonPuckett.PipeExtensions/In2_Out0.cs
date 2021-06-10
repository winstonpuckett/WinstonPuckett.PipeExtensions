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
        /// Pass input to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, Task> asyncFunc)
            => await asyncFunc(input.Item1, input.Item2);

        /// <summary>
        /// Await inputTask, destructure it, and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Action<T, U> func)
            => (await inputTask).Pipe(func);

        /// <summary>
        /// Await inputTask, destructure it, and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, Task> func)
            => await (await inputTask).PipeAsync(func);

        /// <summary>
        /// Await inputTask and pass it to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="input">The object you're operating on wrapped in a Task.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task PipeAsync<T, U>(this (T, U) input, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await input.PipeAsync(i => asyncFunc(i.Item1, i.Item2, cancellationToken));

        /// <summary>
        /// Await inputTask and pass it to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task PipeAsync<T, U>(this Task<(T, U)> inputTask, Func<T, U, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await (await inputTask).PipeAsync(asyncFunc, cancellationToken);
    }
}
