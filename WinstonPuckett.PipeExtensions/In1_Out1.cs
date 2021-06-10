using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class In1_Out1
    {
        /// <summary>
        /// Pass input to func and get back an object of type U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        /// <returns>An object of type U</returns>
        public static U Pipe<T, U>(this T input, Func<T, U> func)
            => func(input);

        /// <summary>
        /// Pass input to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this T input, Func<T, Task<U>> asyncFunc)
            => await asyncFunc(input);

        /// <summary>
        /// Pass input to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this T input, Func<T, CancellationToken, Task<U>> asyncFunc, CancellationToken cancellationToken = default)
            => await asyncFunc(input, cancellationToken);

        /// <summary>
        /// Await inputTask and pass it to func, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on input.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, U> func)
            => func(await inputTask);

        /// <summary>
        /// Await inputTask and pass it to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, Task<U>> asyncFunc)
            => await asyncFunc(await inputTask);

        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, CancellationToken, Task<U>> asyncFunc, CancellationToken cancellationToken = default)
            => await asyncFunc(await inputTask, cancellationToken);
    }
}
