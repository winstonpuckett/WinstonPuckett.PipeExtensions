using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class In1_Out0
    {
        /// <summary>
        /// Pass input to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static void Pipe<T>(this T input, Action<T> func)
            => func(input);

        /// <summary>
        /// Await inputTask and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T>(this Task<T> inputTask, Action<T> func)
            => func(await inputTask);

        /// <summary>
        /// Pass input to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T>(this T input, Func<T, Task> asyncFunc)
            => await asyncFunc(input);

        /// <summary>
        /// Pass input and cancellationToken to asyncFunc
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <param name="cancellationToken">The cancellationToken to pass to asyncFunc</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T>(this T input, Func<T, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await asyncFunc(input, cancellationToken);

        /// <summary>
        /// Pass input and cancellationToken to asyncFunc
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <param name="cancellationToken">The cancellationToken to pass to asyncFunc</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T>(this Task<T> input, Func<T, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken = default)
            => await asyncFunc(await input, cancellationToken);
    }
}
