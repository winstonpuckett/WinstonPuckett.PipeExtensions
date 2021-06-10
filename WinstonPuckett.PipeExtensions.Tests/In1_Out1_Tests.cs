using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In1_Out1_Tests
    {
        [Fact]
        public void T_U()
        {
            int x = 0
                .Pipe((z) => z + 1);

            Assert.Equal(1, x);
        }

        [Fact]
        public void T_U__U_V()
        {
            string s = 10
                .Pipe((num) => num + 20)
                .Pipe((num) => num.ToString());

            Assert.Equal("30", s);
        }

        [Fact]
        public async Task T_TaskU()
        {
            var waitLengthMilliseconds = 30;
            async Task<T> WaitThenReturn<T>(T input)
            {
                await Task.Delay(waitLengthMilliseconds);
                return input;
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var s = await 10
                .PipeAsync(WaitThenReturn);
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= waitLengthMilliseconds);
        }

        [Fact]
        public async Task TCancellationToken_TaskU_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(bool someBool, CancellationToken token) 
                => await Task.Run(() => someBool, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await true.PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task TaskT_TaskU()
        {
            async Task<int> addOneAsync(int i) 
                => await Task.Run(() => i + 1);

            var two = 
                await Task.Run(() => 1)
                    .PipeAsync(addOneAsync);

            Assert.Equal(2, two);
        }

        [Fact]
        public async Task TaskTCancellationToken_TaskU()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<int> addOneAsync(int i, CancellationToken token)
                => await Task.Run(() => i + 1, token);

            var two =
                
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await Task.Run(() => 1).PipeAsync(addOneAsync, cancellationToken));
        }
    }
}
