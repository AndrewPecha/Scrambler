using System;
using System.Collections.Generic;
using Scrambler;
using Scrambler.ValueScramblers.Implementation;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new List<TestClass>{
                new TestClass 
                {
                    TestString = "foo",
                    TestInt = 10,
                    TestDate = DateTime.Now
                },
                new TestClass 
                {
                    TestString = "foo",
                    TestInt = 20,
                    TestDate = DateTime.Now
                },
                new TestClass 
                {
                    TestString = "bar",
                    TestInt = 30,
                    TestDate = DateTime.Now
                }
            };

            var tracker = new ScrambleMapTracker();
            tracker.RegisterMap("testMap", new ScrambleMap()
                .ConditionalReplace<TestClass>(x => x.TestString, "foo", "bar"));

            var scrambler = new DataScrambler(tracker);
                        
            // scrambler.ScrambleEnumerable(foo, x => 
            // {
            //     x.SetReplaceAll();
            //     //x.ConditionalReplace<TestClass>(x => x.TestString, "foo", "bar");
            //     x.AddCustomScrambler(typeof(int), new CustomIntScrambler());
            // });

            scrambler.ScrambleEnumerable(foo, "testMap");

            foreach (var bar in foo)
            {
                Console.WriteLine($"{bar.TestString}\n{bar.TestInt}\n{bar.TestDate}");
            }

            scrambler.UnscrambleEnumerable(foo, "testMap");

            foreach (var bar in foo)
            {
                Console.WriteLine($"{bar.TestString}\n{bar.TestInt}\n{bar.TestDate}");
            }
        }

        public class TestClass 
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }
            public DateTime TestDate { get; set; }
        }
    }
}
