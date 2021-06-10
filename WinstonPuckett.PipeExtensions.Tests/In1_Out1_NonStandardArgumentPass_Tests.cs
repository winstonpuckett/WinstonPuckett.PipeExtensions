using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class In1_Out1_NonStandardArgumentPass_Tests
    {


        [Fact]
        public void CanPassTupleToFunctionExpectingTuple()
        {
            static (string name, int age) addOneToAge((string name, int age) person)
                => (person.name, person.age + 1);

            var person =
                (name: "Ingrid", age: 10)
                .Pipe(addOneToAge)
                .Pipe(addOneToAge)
                .Pipe(addOneToAge);

            Assert.Equal(13, person.age);
        }


        [Fact]
        public void CanPassExtraParameterToAnonymousFunction()
        {
            static int AddStepsToTotal(int stepsTravelled, int stepsToAddPerCall)
                => stepsTravelled + stepsToAddPerCall;

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

        [Fact]
        public void CanPassFromScopeToNamedFunction()
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
