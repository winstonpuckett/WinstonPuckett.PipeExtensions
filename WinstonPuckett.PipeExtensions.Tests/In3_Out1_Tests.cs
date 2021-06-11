using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In3_Out1_Tests
    {
        [Fact]
        public void A_B()
        {
            static bool returnTrue(int _, string _2, char _3)
                => true;

            var result =
                (0, string.Empty, 'b')
                .Pipe(returnTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskA_B()
        {
            static bool flipToTrue(int _, string _2, char _3)
                => true;

            var result =
                await Task.Run(() => (0, string.Empty, 'b'))
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task A_TaskB()
        {
            static async Task<bool> flipToTrue(int _, string _2, char _3) => await Task.Run(() => true);

            var result = await
                (0, string.Empty, 'b')
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskA_TaskB()
        {
            static async Task<bool> flipToTrue(int _, string _2, char _3)
                => await Task.Run(() => true);

            var testBool = await
                Task.Run(() => (0, string.Empty, 'b'))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task ACancellationToken_TaskB_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(int _, string _2, char _3, CancellationToken token)
                => await Task.Run(() => true, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => { var x = await (0, string.Empty, 'b').PipeAsync(canCancelFunc, cancellationToken); });
        }

        [Fact]
        public async Task TaskACancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(int _, string _2, char _3, CancellationToken token)
                => await Task.Run(() => true, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () =>
                {
                    var x = await
                        Task.Run(() => (0, string.Empty, 'b'))
                        .PipeAsync(canCancelFunc, cancellationToken);
                });
        }
    }
}
