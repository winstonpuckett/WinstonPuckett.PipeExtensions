using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In2_Out0_Tests
    {
        [Fact]
        public void T()
        {
            var testBool = false;
            void flipToTrue(int anyNum, string anyString) { testBool = true; };

            (1, "Charlie")
                .Pipe(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TaskT()
        {
            var testBool = false;
            void flipToTrue(int anyNum, string anyString) { testBool = true; };

            await Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task T_Task()
        {
            var testBool = false;
            async Task flipToTrue(int anyNum, string anyString) { await Task.Run(() => testBool = true); };

            await (1, "Charlie")
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TaskT_Task()
        {
            var testBool = false;
            async Task flipToTrue(int _, string _2) 
                => await Task.Run(() => testBool = true);

            await Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TCancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(string _, int _2, CancellationToken token)
                => await Task.Run(() => { }, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await ("", 0).PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task TaskTCancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(string _, int _2, CancellationToken token)
                => await Task.Run(() => { }, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await Task.Run(()=>("", 0)).PipeAsync(canCancelFunc, cancellationToken));
        }
    }
}
