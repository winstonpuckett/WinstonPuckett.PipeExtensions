using System;
using System.Threading.Tasks;

namespace WinstonPuckett.PipeExtensions
{
    public static class PipeExtensions
    {
        public static U Pipe<T, U>(this T input, Func<T, U> @operator)
        {
            return @operator(input);
        }

        public static void Pipe<T>(this T input, Action<T> @operator)
        {
            @operator(input);
        }

        public static async Task<U> PipeAsync<T, U>(this T input, Func<T, Task<U>> @operator)
        {
            return await @operator(input);
        }
        public static async Task PipeAsync<T>(this T input, Func<T, Task> @operator)
        {
            await @operator(input);
        }
    }
}
