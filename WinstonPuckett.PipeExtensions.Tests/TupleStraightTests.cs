using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class TupleStraightTests
    {
        [Fact]
        public void NamedTuplesWork()
        {
            var person = 
                (name: "Ingrid", age: 10)
                .Pipe(AddOneToAge)
                .Pipe(AddOneToAge)
                .Pipe(AddOneToAge);

            Assert.Equal(13, person.age);
        }

        private (string name, int age) AddOneToAge((string name, int age) person)
        {
            return (person.name, person.age + 1);
        }

        [Fact]
        public void PassingFromScopedWorks()
        {
            int stepsTravelled = 0;
            int stepsToAddPerCall = 2;

            var totalSteps = 
                stepsTravelled
                .Pipe(travelled => AddStepsToTotal(travelled, stepsToAddPerCall))
                .Pipe(travelled => AddStepsToTotal(travelled, stepsToAddPerCall))
                .Pipe(travelled => AddStepsToTotal(travelled, stepsToAddPerCall))
                .Pipe(travelled => AddStepsToTotal(travelled, stepsToAddPerCall))
                .Pipe(travelled => AddStepsToTotal(travelled, stepsToAddPerCall));

            Assert.Equal(10, totalSteps);
        }

        private int AddStepsToTotal(int stepsTravelled, int stepsToAddPerCall)
        {
            return stepsTravelled + stepsToAddPerCall;
        }

        [Fact]
        public void PassingFromScopedWorks_RefactoredAsMonadic()
        {
            int stepsTravelled = 0;
            int stepsToAddPerCall = 2;

            int AddStepsToStepsTravelled(int travelled)
                => travelled + stepsToAddPerCall;

            var totalSteps =
                stepsTravelled
                .Pipe(AddStepsToStepsTravelled)
                .Pipe(AddStepsToStepsTravelled)
                .Pipe(AddStepsToStepsTravelled)
                .Pipe(AddStepsToStepsTravelled)
                .Pipe(AddStepsToStepsTravelled);

            Assert.Equal(10, totalSteps);
        }
    }
}
