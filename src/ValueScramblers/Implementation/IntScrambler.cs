using Scrambler.ValueScramblers.Interfaces;
using System.Reflection;
using System;

namespace Scrambler.ValueScramblers.Implementation
{
    public class IntScrambler : IIntScrambler
    {
        public void ScrambleValue(object input, PropertyInfo propInfo)
        {
            propInfo.SetValue(input, Convert.ToInt32(Convert.ToInt32(propInfo.GetValue(input)) * 1.1));
        }

        public void UnscrambleValue(object input, PropertyInfo propInfo)
        {
            
        }
    }
}