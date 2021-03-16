using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Scrambler.ValueScramblers.Interfaces;

namespace Scrambler
{
    public class ScrambleMap
    {
        internal Dictionary<string, ReplaceCondition> ReplaceConditions { get; private set; }
        internal Dictionary<Type, IScrambler> CustomScramblers { get; private set;}
        internal bool ReplaceAll { get; private set; }

        public ScrambleMap()
        {
            ReplaceConditions = new Dictionary<string, ReplaceCondition>();
            CustomScramblers = new Dictionary<Type, IScrambler>();
        }

        public ScrambleMap ConditionalReplace<T>(Expression<Func<T, object>> expression, object oldValue, object newValue)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                var replaceCondition = new ReplaceCondition(oldValue, newValue);

                ReplaceConditions.Add(memberExpression.Member.Name, replaceCondition);
            }

            return this;
        }

        public ScrambleMap AddCustomScrambler(Type type, IScrambler customScrambler)
        {
            CustomScramblers.Add(type, customScrambler);
            return this;
        }

        public ScrambleMap SetReplaceAll(bool replaceAll = true)
        {
            ReplaceAll = replaceAll;
            return this;
        }

        internal ReplaceCondition FindReplaceCondition(string propName)
        {
            if (ReplaceConditions.TryGetValue(propName, out ReplaceCondition replaceCondition))
            {
                return replaceCondition;
            }

            return null;
        }

        internal bool HasCustomScrambler(Type typeToScramble)
        {
            return (CustomScramblers.TryGetValue(typeToScramble, out IScrambler customeScrambler));
        }

        internal IScrambler GetCustomScrambler(Type typeToScramble)
        {
            if(CustomScramblers.TryGetValue(typeToScramble, out IScrambler customScrambler))
                return customScrambler;

                return null;
        }
    }

    internal class ReplaceCondition
    {
        public object ConditonValue { get; set; }
        public object ReplaceValue { get; set; }

        public ReplaceCondition(object conditionValue, object replaceValue)
        {
            ConditonValue = conditionValue;
            ReplaceValue = replaceValue;
        }
    }
}
