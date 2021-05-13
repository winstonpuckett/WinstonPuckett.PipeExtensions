using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class PipeExtensionTests
    {
        [Fact]
        public void Pipe_ReturnsResultOfOperandPlusFunction()
        {
            int x = 0
                .Pipe((z) => z + 1);
            
            Assert.Equal(1, x);
        }

        [Fact]
        public void Pipe_CanConcatenateDifferentTypes()
        {
            string s = 10
                .Pipe((num) => num + 20)
                .Pipe((num) => num.ToString());

            Assert.Equal("30", s);
        }

        [Fact]
        public void Pipe_CanUseActionWhenNoReturnNeeded()
        {
            void DoNothing(object o) { }
            "This is any string"
                .Pipe(s => s.Length)
                .Pipe(s => DoNothing(s));
        }

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
        public void Pipe_SimpleUserFlow()
        {
            var input = new InputModel { Id = 1 };

            // Good
            input
                .Pipe(Query)
                .Pipe(Validate)
                .Pipe(Transform)
                .Pipe(Submit);

            // Eh
            var queryResult = Query(input);
            Validate(queryResult);
            var transform = Transform(queryResult);
            Submit(transform);

            // Bad
            Submit(Transform(Validate(Query(input))));
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
        void Submit(OutputModel output)
        {

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
