using System;
using Scrambler;
using Scrambler.ValueScramblers.Implementation;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new TestClass 
            {
                TestString = "foo",
                TestInt = 10,
                TestDate = DateTime.Now
            };

            var scrambler = new DataScrambler(new StringScrambler(), new IntScrambler(), new DateScrambler());
            
            scrambler.Scramble(foo, x => 
            {
                x.SetReplaceAll();
                x.ConditionalReplace<TestClass>(x => x.TestString, "foo", "bar");
                x.AddCustomScrambler(typeof(int), new CustomIntScrambler());
            });            

            Console.WriteLine($"{foo.TestString}\n{foo.TestInt}\n{foo.TestDate}");
        }

        public class TestClass 
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }
            public DateTime TestDate { get; set; }
        }
    }
}
