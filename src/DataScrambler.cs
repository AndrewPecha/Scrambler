using System;
using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using System.Collections.Generic;

namespace Scrambler
{
    public class DataScrambler
    {
        private IStringScrambler _stringScrambler;
        private IIntScrambler _intScrambler;
        private IDateScrambler _dateScrambler;
        private Dictionary<Type, IScrambler> _valueScramblers;

        public DataScrambler(IStringScrambler stringScrambler, IIntScrambler intScrambler,
            IDateScrambler dateScrambler)
        {
            _valueScramblers = new Dictionary<Type, IScrambler>();
            _valueScramblers[typeof(string)] = stringScrambler;
            _valueScramblers[typeof(int)] = intScrambler;
            _valueScramblers[typeof(DateTime)] = dateScrambler;
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
            var scrambler = _valueScramblers[propInfo.PropertyType];
            scrambler.ScrambleValue(input, propInfo, customAppend);
        }

        public T Scramble<T>(T objectToScramble, Action<ScrambleMap> scrambleMapConfigurator)
        {
            var props = objectToScramble.GetType().GetProperties();
            var scrambleMap = new ScrambleMap();

            scrambleMapConfigurator?.Invoke(scrambleMap);

            for (int i = 0; i < props.Length; i++)
            {
                var replaceCondition = scrambleMap.FindReplaceCondition(props[i].Name);

                if(replaceCondition?.ConditonValue.Equals(props[i].GetValue(objectToScramble)) ?? false)
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