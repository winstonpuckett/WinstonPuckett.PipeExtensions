using System;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class PipeExtensions
    {
        /// <summary>
        /// Pass T to func and get back U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="func">The function to call which operates on T.</param>
        /// <returns>An object of type U</returns>
        public static U Pipe<T, U>(this T input, Func<T, U> func)
        {
            return func(input);
        }

        /// <summary>
        /// Pass T to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="func">The function to call which operates on T.</param>
        public static void Pipe<T>(this T input, Action<T> func)
        {
            func(input);
        }

        /// <summary>
        /// Await T and pass it to func, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, U> func)
        {
            return func(await inputTask);
        }

        /// <summary>
        /// Await T and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="func">The function to call which operates on T.</param>
        public static async Task PipeAsync<T>(this Task<T> inputTask, Action<T> func)
        {
            func(await inputTask);
        }

        /// <summary>
        /// Await T and pass it to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, Task<U>> asyncFunc)
        {
            return await asyncFunc(await inputTask);
        }

        /// <summary>
        /// Pass T to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this T input, Func<T, Task<U>> asyncFunc)
        {
            return await asyncFunc(input);
        }

        /// <summary>
        /// Await T and pass it to asyncFunc, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="asyncFunc">The function to call which operates on T.</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T>(this T input, Func<T, Task> asyncFunc)
        {
            await asyncFunc(input);
        }
    }
}
