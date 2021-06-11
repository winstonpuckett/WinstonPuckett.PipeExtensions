using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In1_Out0_Tests
    {
        [Fact]
        public void A()
        {
            bool testBool = false;
            void flipBool(int _) { testBool = true; }
            "This is any string"
                .Pipe(s => s.Length)
                .Pipe(flipBool);

            Assert.True(testBool);
        }

        [Fact]
        public async Task ACancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(int number, CancellationToken token)
                => await Task.Run(() => { }, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await 10.PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task A_Task()
        {
            var waitLengthMilliseconds = 30;
            async Task waitThenNothing<T>(T input) 
                => await Task.Delay(waitLengthMilliseconds);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            await 10
                .PipeAsync(waitThenNothing);
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= waitLengthMilliseconds);
        }

        [Fact]
        public async Task TaskA_Task()
        {
            var waitLengthMilliseconds = 30;
            async Task waitThenNothing(int input)
            {
                await Task.Delay(waitLengthMilliseconds);
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            await Task.Run(() => 0)
                .PipeAsync(waitThenNothing);
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= waitLengthMilliseconds);
        }
    }
}
