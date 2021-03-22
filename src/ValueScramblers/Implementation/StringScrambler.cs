using System.Reflection;
using System.Collections.Generic;
using Scrambler.ValueScramblers.Interfaces;

namespace Scrambler.ValueScramblers.Implementation
{
    public class StringScrambler : IStringScrambler
    {
        private List<string> _existingKeyTracker;

        public StringScrambler()
        {
            _existingKeyTracker = new List<string>();
        }

        public void ScrambleValue(object input, PropertyInfo propInfo)
        {
            var occurences = GetGroupNumber((string)propInfo.GetValue(input));
            propInfo.SetValue(input, propInfo.Name + occurences);
        }

        private int GetGroupNumber(string key)
        {
            if(_existingKeyTracker.Contains(key))
                return _existingKeyTracker.IndexOf(key) + 1;

            _existingKeyTracker.Add(key);
            return _existingKeyTracker.IndexOf(key) + 1;
        }
    }
}