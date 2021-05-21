using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class BasicPipeOperationsTests
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
    }
}
