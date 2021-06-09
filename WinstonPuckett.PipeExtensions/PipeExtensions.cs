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
        /// Destructure input tuple and pass it to func.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter to func.</typeparam>
        /// <param name="input">The object passed to func.</param>
        /// <param name="func">The function to call which operates on input.</param>
        public static void Pipe<T, U>(this (T, U) input, Action<T, U> func)
            => func(input.Item1, input.Item2);
    }
}
