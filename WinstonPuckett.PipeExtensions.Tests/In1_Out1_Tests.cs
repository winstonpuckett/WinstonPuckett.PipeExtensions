using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In1_Out1_Tests
    {
        [Fact]
        public void A_B()
        {
            int x = 0
                .Pipe((z) => z + 1);

            Assert.Equal(1, x);
        }

        [Fact]
        public void A_B_Then_B_C()
        {
            string s = 10
                .Pipe((num) => num + 20)
                .Pipe((num) => num.ToString());

            Assert.Equal("30", s);
        }

        [Fact]
        public async Task A_TaskB()
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
        public async Task ACancellationToken_TaskB_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(bool someBool, CancellationToken token) 
                => await Task.Run(() => someBool, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await true.PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task TaskA_TaskB()
        {
            async Task<int> addOneAsync(int i) 
                => await Task.Run(() => i + 1);

            var two = 
                await Task.Run(() => 1)
                    .PipeAsync(addOneAsync);

            Assert.Equal(2, two);
        }

        [Fact]
        public async Task TaskACancellationToken_TaskB()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<int> addOneAsync(int i, CancellationToken token)
                => await Task.Run(() => i + 1, token);

            var two =
                
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await Task.Run(() => 1).PipeAsync(addOneAsync, cancellationToken));
        }
    }
}
