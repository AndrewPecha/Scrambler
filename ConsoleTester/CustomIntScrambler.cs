using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using System;

namespace ConsoleTester
{
    public class CustomIntScrambler : IIntScrambler
    {
        public void ScrambleValue(object input, PropertyInfo propInfo)
        {
            propInfo.SetValue(input, Convert.ToInt32(Convert.ToInt32(propInfo.GetValue(input)) * 1.2));
        }

        public void UnscrambleValue(object input, PropertyInfo propInfo)
        {
            propInfo.SetValue(input, Convert.ToInt32(Convert.ToInt32(propInfo.GetValue(input)) / 1.2));
        }
    }
}