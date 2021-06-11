﻿using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In2_Out1_Tests
    {
        [Fact]
        public void T()
        {
            bool returnTrue(int _, string _2)
                => true;

            var result = 
                (0, string.Empty)
                .Pipe(returnTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskT()
        {
            bool flipToTrue(int anyNum, string anyString) 
                => true;

            var result = 
                await Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task T_Task()
        {
            async Task<bool> flipToTrue(int anyNum, string anyString) => await Task.Run(() => true);
            var result = await 
                (0,string.Empty)
                .PipeAsync(flipToTrue);

            await (1, "Charlie")
                .PipeAsync(flipToTrue);

            Assert.True(result);
        }

        [Fact]
        public async Task TaskT_Task()
        {
            static async Task<bool> flipToTrue(int _, string _2)
                => await Task.Run(() => true);

            var testBool = await 
                Task.Run(() => (1, "Charlie"))
                .PipeAsync(flipToTrue);

            Assert.True(testBool);
        }

        [Fact]
        public async Task TCancellationToken_Task_ExceptionThrown()
        {
            var cancellationToken = new CancellationToken(true);

            async Task<bool> canCancelFunc(string _, int _2, CancellationToken token)
                => await Task.Run(() => true, cancellationToken);

            await Assert.ThrowsAsync<TaskCanceledException>(async () => { var x = await ("", 0).PipeAsync(canCancelFunc, cancellationToken); });
        }

        [Fact]
        public async Task TaskTCancellationToken_Task_ExceptionThrown()
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