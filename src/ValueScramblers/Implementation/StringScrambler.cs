using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;

namespace Scrambler.ValueScramblers.Implementation
{
    public class StringScrambler : IStringScrambler
    {
        public void ScrambleValue(object input, PropertyInfo propInfo)
        {
            propInfo.SetValue(input, propInfo.Name);
        }
    }
}