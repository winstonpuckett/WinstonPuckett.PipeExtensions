using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In3_Out0_Tests
    {
        [Fact]
        public void A()
        {
            var testBool = false;
            void flipToTrue(int _, string _2, char _3)
            { testBool = true; };

            (0, string.Empty, 'b')
                .Pipe(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TaskA()
        {
            var testBool = false;
            void flipToTrue(int _, string _2, char _3)
            { testBool = true; };

            await Task.Run(() => (0, string.Empty, 'b'))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task A_Task()
        {
            var testBool = false;
            async Task flipToTrue(int _, string _2, char _3)
            { await Task.Run(() => testBool = true); };

            await (0, string.Empty, 'b')
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TaskA_Task()
        {
            var testBool = false;
            async Task flipToTrue(int _, string _2, char _3)
                => await Task.Run(() => testBool = true);

            await Task.Run(() => (0, string.Empty, 'b'))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task ACancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(int _, string _2, char _3, CancellationToken token)
                => await Task.Run(() => { }, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await (0, string.Empty, 'b').PipeAsync(canCancelFunc, cancellationToken));
        }

        [Fact]
        public async Task TaskACancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task canCancelFunc(int _, string _2, char _3, CancellationToken token)
                => await Task.Run(() => { }, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => await Task.Run(() => (0, string.Empty, 'b')).PipeAsync(canCancelFunc, cancellationToken));
        }
    }
}
