using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class AsyncTests
    {
        [Fact]
        public async Task Pipe_CanUseAsyncFunc()
        {
            var waitLengthMilliseconds = 200;
            async Task<T> WaitThenReturn<T>(T input)
            {
                await Task.Delay(waitLengthMilliseconds);
                return input;
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var s = await 10
                .PipeAsync(async num => await WaitThenReturn(num));
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= waitLengthMilliseconds);
        }

        [Fact]
        public async Task Pipe_CanUseAsyncAction()
        {
            var waitLengthMilliseconds = 200;
            async Task WaitThenNothing<T>(T input)
            {
                await Task.Delay(waitLengthMilliseconds);
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            await 10
                .PipeAsync(async num => await WaitThenNothing(num));
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= waitLengthMilliseconds);
        }

        [Fact]
        public async Task Pipe_SimpleUserFlow_TwoAsyncMethod()
        {
            var input = new InputModel { Id = 1 };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await input 
                .Pipe(Query)
                .Pipe(Validate)
                .PipeAsync(TransformAsync)
                .PipeAsync(SubmitAsync);
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= 2000);
        }
        
        [Fact]
        public async Task Pipe_SimpleUserFlow_MixedAsync()
        {
            var input = new InputModel { Id = 1 };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await input
                .Pipe(Query)
                .Pipe(Validate)
                .PipeAsync(TransformAsync)
                .PipeAsync(Submit);
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds >= 2000);
        }

        Model Query(InputModel input)
        {
            return new Model
            {
                Id = input.Id,
                Email = "test@foo.com",
                Name = "Test Name"
            };
        }
        Model Validate(Model model)
        {
            return model;
        }
        OutputModel Transform(Model model)
        {
            return new OutputModel { Email = model.Email };
        }
        async Task<OutputModel> TransformAsync(Model model)
        {
            await Task.Delay(2000);
            return new OutputModel { Email = model.Email };
        }
        void Submit(OutputModel output)
        {

        }
        async Task SubmitAsync(OutputModel output)
        {
            await Task.Delay(2000);
        }

        class InputModel
        {
            public int Id { get; set; }
        }
        class Model
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
        }
        class OutputModel
        {
            public string Email { get; set; }
        }
    }
}
