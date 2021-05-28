using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class CancellationTokenTests
    {
        [Fact]
        public async Task HonoredForNoReturnType()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(int number, CancellationToken token)
                => await Task.Run(() => number, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await 10.PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task HonoredForBoolReturnType()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(bool someBool, CancellationToken token)
                => await Task.Run(() => someBool, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await true.PipeAsync(canCancelFunc, cancellationToken));
        }
    }
}
