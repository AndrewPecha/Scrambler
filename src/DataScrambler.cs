using System;
using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using System.Collections.Generic;

namespace Scrambler
{
    public class DataScrambler
    {
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
                ScrambleProp(input, props[i]);
            }

            return input;
        }

        private void ScrambleProp(object input, PropertyInfo propInfo)
        {
            var scrambler = _valueScramblers[propInfo.PropertyType];
            scrambler.ScrambleValue(input, propInfo);
        }

        public IEnumerable<T> ScrambleEnumerable<T>(IEnumerable<T> collectionToScramble, Action<ScrambleMap> scrambleMapConfigurator)
        {
            var props = collectionToScramble.GetType().GetGenericArguments()[0].GetProperties();
            var scrambleMap = new ScrambleMap();

            scrambleMapConfigurator?.Invoke(scrambleMap);

            foreach (var objectToScramble in collectionToScramble)
            {
                ScrambleObjectProperties(objectToScramble, props, scrambleMap);
            }

            return collectionToScramble;
        }

        public T Scramble<T>(T objectToScramble, Action<ScrambleMap> scrambleMapConfigurator)
        {
            var props = objectToScramble.GetType().GetProperties();
            var scrambleMap = new ScrambleMap();

            scrambleMapConfigurator?.Invoke(scrambleMap);

            ScrambleObjectProperties(objectToScramble, props, scrambleMap);

            return objectToScramble;
        }

        private void ScrambleObjectProperties<T>(T objectToScramble, PropertyInfo[] props, ScrambleMap scrambleMap)
        {
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
                    customScrambler.ScrambleValue(objectToScramble, props[i]);
                }
                else if (scrambleMap.ReplaceAll)
                {
                    ScrambleProp(objectToScramble, props[i]);
                }
            }
        }
    }
}