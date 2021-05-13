using System;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class PipeExtensions
    {
        /// <summary>
        /// Take in a function which accepts T returns U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="operator">The function to call which operates on T.</param>
        /// <returns>An object of type U</returns>
        public static U Pipe<T, U>(this T input, Func<T, U> @operator)
        {
            return @operator(input);
        }

        /// <summary>
        /// Take in a function which accepts T returns nothing.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="operator">The function to call which operates on T.</param>
        public static void Pipe<T>(this T input, Action<T> @operator)
        {
            @operator(input);
        }

        /// <summary>
        /// Take in a Task of T, await T and pass it to @operator, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="operator">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, U> @operator)
        {
            return @operator(await inputTask);
        }

        /// <summary>
        /// Take in a Task of T, await T and pass it to @operator.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        public static async Task PipeAsync<T>(this Task<T> inputTask, Action<T> @operator)
        {
            @operator(await inputTask);
        }

        /// <summary>
        /// Take in a Task of T, await T and pass it to @asyncOperator, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="inputTask">The object you're operating on wrapped in a Task.</param>
        /// <param name="asyncOperator">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this Task<T> inputTask, Func<T, Task<U>> asyncOperator)
        {
            return await asyncOperator(await inputTask);
        }

        /// <summary>
        /// Take in an object of type T, await T and pass it to @asyncOperator, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <typeparam name="U">The type this Pipe returns</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="asyncOperator">The function to call which operates on T.</param>
        /// <returns>An object of type U wrapped in a Task</returns>
        public static async Task<U> PipeAsync<T, U>(this T input, Func<T, Task<U>> asyncOperator)
        {
            return await asyncOperator(input);
        }

        /// <summary>
        /// Take in an object of type T, await T and pass it to @asyncOperator, return a Task of U.
        /// </summary>
        /// <typeparam name="T">The type you're operating on.</typeparam>
        /// <param name="input">The object you're operating on.</param>
        /// <param name="asyncOperator">The function to call which operates on T.</param>
        /// <returns>A Task</returns>
        public static async Task PipeAsync<T>(this T input, Func<T, Task> asyncOperator)
        {
            await asyncOperator(input);
        }
    }
}
