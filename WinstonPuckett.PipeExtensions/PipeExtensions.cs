using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class PipeExtensions
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
        /// Pass input to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static void Pipe<T>(this T input, Action<T> func)
            => func(input);

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
        /// Await inputTask and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static async Task PipeAsync<T>(this Task<T> inputTask, Action<T> func)
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
        public static async Task PipeAsync<T>(this T input, Func<T, CancellationToken, Task> asyncFunc, CancellationToken cancellationToken)
            => await asyncFunc(input, cancellationToken);

        /// <summary>
        /// Pass input and cancellationToken to asyncFunc and get back a Task of U
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type asyncFunc returns</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <param name="cancellationToken">The cancellationToken to pass to asyncFunc</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task PipeAsync<T, U>(this T input, Func<T, CancellationToken, Task<U>> asyncFunc, CancellationToken cancellationToken)
            => await asyncFunc(input, cancellationToken);
    }
}
