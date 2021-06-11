using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class SampleUserFlow_Tests
    {
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

            Assert.True(stopwatch.ElapsedMilliseconds >= 50);
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

            Assert.True(stopwatch.ElapsedMilliseconds >= 50);
        }

        #region Example Infrastructure
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

        async Task<OutputModel> TransformAsync(Model model)
        {
            await Task.Delay(50);
            return new OutputModel { Email = model.Email };
        }
        void Submit(OutputModel output)
        {

        }
        async Task SubmitAsync(OutputModel output)
        {
            await Task.Delay(50);
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
        #endregion
    }
}
