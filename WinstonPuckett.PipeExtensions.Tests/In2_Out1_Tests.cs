using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In2_Out1_Tests
    {
        [Fact]
        public void A_B()
        {
            static bool returnTrue(int _, string _2)
                => true;

            var result = 
                (0, string.Empty)
                .Pipe(returnTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskA_B()
        {
            static bool flipToTrue(int anyNum, string anyString) 
                => true;

            var result = 
                await Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task A_TaskB()
        {
            static async Task<bool> flipToTrue(int anyNum, string anyString) 
                => await Task.Run(() => true);

            var result = await 
                (0,string.Empty)
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskA_TaskB()
        {
            static async Task<bool> flipToTrue(int _, string _2)
                => await Task.Run(() => true);

            var testBool = await 
                Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task ACancellationToken_TaskB_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(string _, int _2, CancellationToken token)
                => await Task.Run(() => true, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => { var x = await ("", 0).PipeAsync(canCancelFunc, cancellationToken); });
        }

        [Fact]
        public async Task TaskACancellationToken_TaskB_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(string _, int _2, CancellationToken token)
                => await Task.Run(() => true, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () =>
                {
                    var x = await 
                        Task.Run(() => ("", 0))
                        .PipeAsync(canCancelFunc, cancellationToken);
                });
        }
    }
}
