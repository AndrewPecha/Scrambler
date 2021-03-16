using System;
using Scrambler;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new TestClass 
            {
                TestString = "foo",
                TestInt = 10
            };

            var scrambler = new DataScrambler();
            // scrambler.Scramble(foo);            
            
            scrambler.Scramble(foo, x => 
            {
                x.ConditionalReplace<TestClass>(x => x.TestString, "foo", "bar");
                x.AddCustomScrambler(typeof(int), new CustomIntScrambler());
            });            

            Console.WriteLine($"{foo.TestString}\n{foo.TestInt}");
        }

        public class TestClass 
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }
        }
    }
}
