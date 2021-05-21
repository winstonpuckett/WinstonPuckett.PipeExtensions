using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class MultipleArgumentTests
    {
        [Fact]
        public void CanPassMultipleArguments()
        {
            var person = 
                (name: "Ingrid", age: 10)
                .Pipe(AddOneToAge)
                .Pipe(AddOneToAge)
                .Pipe(AddOneToAge);

            Assert.Equal(13, person.age);
        }

        public (string name, int age) AddOneToAge((string name, int age) person)
        {
            return (person.name, person.age + 1);
        }
    }
}
