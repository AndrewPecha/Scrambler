using System;
using System.Reflection;
using Scrambler.ValueScramblers.Interfaces;
using System.Collections.Generic;

namespace Scrambler
{
    public class DataScrambler
    {
        private Dictionary<Type, IScrambler> _valueScramblers;
        private ScrambleMapTracker _mapTracker;

        public DataScrambler(ScrambleMapTracker mapTracker)
        {
            _valueScramblers = new Dictionary<Type, IScrambler>();
            _mapTracker = mapTracker;
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
            _valueScramblers.TryGetValue(propInfo.PropertyType, out var scrambler);

            if(scrambler != null)
                scrambler.ScrambleValue(input, propInfo);
        }

        public IEnumerable<T> ScrambleEnumerable<T>(IEnumerable<T> collectionToScramble, Action<ScrambleMap> scrambleMapConfigurator)
        {
            var scrambleMap = new ScrambleMap();

            scrambleMapConfigurator?.Invoke(scrambleMap);

            foreach (var objectToScramble in collectionToScramble)
            {
                Scramble(objectToScramble, scrambleMap);
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

        public IEnumerable<T> ScrambleEnumerable<T>(IEnumerable<T> collectionToScramble, ScrambleMap scrambleMap)
        {
            foreach (var objectToScramble in collectionToScramble)
            {
                Scramble(objectToScramble, scrambleMap);
            }

            return collectionToScramble;
        }

        public T Scramble<T>(T objectToScramble, ScrambleMap scrambleMap)
        {
            var props = objectToScramble.GetType().GetProperties();

            ScrambleObjectProperties(objectToScramble, props, scrambleMap);

            return objectToScramble;
        }

        public IEnumerable<T> ScrambleEnumerable<T>(IEnumerable<T> collectionToScramble, string mapKey)
        {
            var scrambleMap = _mapTracker.GetMap(mapKey);

            foreach (var objectToScramble in collectionToScramble)
            {
                Scramble(objectToScramble, scrambleMap);
            }

            return collectionToScramble;
        }

        public T Scramble<T>(T objectToScramble, string mapKey)
        {
            var props = objectToScramble.GetType().GetProperties();
            var scrambleMap = _mapTracker.GetMap(mapKey);

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
            }
        }

        public IEnumerable<T> UnscrambleEnumerable<T>(IEnumerable<T> collectionToUnscramble, string mapKey)
        {
            var scrambleMap = _mapTracker.GetMap(mapKey);

            foreach (var objectToUnscramble in collectionToUnscramble)
            {
                Unscramble(objectToUnscramble, scrambleMap);
            }

            return collectionToUnscramble;
        }

        public T Unscramble<T>(T objectToUnscramble, ScrambleMap scrambleMap)
        {
            var props = objectToUnscramble.GetType().GetProperties();

            UnscrambleObjectProperties(objectToUnscramble, props, scrambleMap);

            return objectToUnscramble;
        }

        private void UnscrambleObjectProperties<T>(T objectToUnscramble, PropertyInfo[] props, ScrambleMap scrambleMap)
        {
            foreach (var prop in props)
            {
                var replaceCondition = scrambleMap.FindReplaceCondition(prop.Name);

                if(replaceCondition?.ReplaceValue.Equals(prop.GetValue(objectToUnscramble)) ?? false)
                {
                    prop.SetValue(objectToUnscramble, replaceCondition.ConditonValue);
                }
                else if(scrambleMap.HasCustomScrambler(prop.PropertyType))
                {
                    var customeScrambler = scrambleMap.GetCustomScrambler(prop.PropertyType);
                    customeScrambler.UnscrambleValue(objectToUnscramble, prop);
                }
            }
        }
    }
}