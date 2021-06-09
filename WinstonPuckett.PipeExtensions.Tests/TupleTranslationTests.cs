using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.PipeExtensions.Tests
{
    public class TupleTranslationTests
    {
        [Fact]
        public void AcceptsForDyadicFunction()
        {
            static void acceptTwoArguments(int id, string name) { };
            (1, "Charlie")
                .Pipe(acceptTwoArguments);
        }
    }
}
