using System.Reflection;

namespace Scrambler.ValueScramblers.Interfaces
{
    public interface IScrambler
    {
         void ScrambleValue(object input, PropertyInfo propInfo);
         void UnscrambleValue(object input, PropertyInfo propInfo);
    }
}