using System;
using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using Scrambler.ValueScramblers.Implementation;

namespace Scrambler
{
    public class DataScrambler
    {
        private IStringScrambler _stringScrambler;
        private IIntScrambler _intScrambler;

        public DataScrambler()
        {
            _stringScrambler = new StringScrambler();
            _intScrambler = new IntScrambler();
        }

        public T Scramble<T>(T input)
        {
            var props = input.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                ScrambleProp(input, props[i], i.ToString());
            }

            return input;
        }

        private void ScrambleProp(object input, PropertyInfo propInfo, string customAppend)
        {
            if (propInfo.PropertyType == typeof(string))
            {
                _stringScrambler.ScrambleValue(input, propInfo, customAppend);
            }
            else if (propInfo.PropertyType == typeof(int))
            {
                _intScrambler.ScrambleValue(input, propInfo, string.Empty);
            }
        }

        public T Scramble<T>(T objectToScramble, Action<ScrambleMap> scrambleMapConfigurator)
        {
            var props = objectToScramble.GetType().GetProperties();
            var scrambleMap = new ScrambleMap();

            scrambleMapConfigurator?.Invoke(scrambleMap);

            for (int i = 0; i < props.Length; i++)
            {
                var replaceCondition = scrambleMap.FindReplaceCondition(props[i].Name);
                if(replaceCondition?.ConditonValue == props[i].GetValue(objectToScramble))
                {
                    props[i].SetValue(objectToScramble, replaceCondition.ReplaceValue);
                }
                else if (scrambleMap.HasCustomScrambler(props[i].PropertyType))
                {
                    var customScrambler = scrambleMap.GetCustomScrambler(props[i].PropertyType);
                    customScrambler.ScrambleValue(objectToScramble, props[i], string.Empty);
                }
                else if (scrambleMap.ReplaceAll)
                {
                    ScrambleProp(objectToScramble, props[i], i.ToString());
                }
            }

            return objectToScramble;
        }
    }
}