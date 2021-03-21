using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using System;

namespace Scrambler.ValueScramblers.Implementation
{
    public class DateScrambler : IDateScrambler
    {
        public void ScrambleValue(object input, PropertyInfo propInfo)
        {
             var date = Convert.ToDateTime(propInfo.GetValue(input));
             date = date.AddYears(1);

             propInfo.SetValue(input, date);
        }
    }
}