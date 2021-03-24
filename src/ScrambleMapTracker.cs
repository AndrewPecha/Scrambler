using System.Collections.Generic;

namespace Scrambler
{
    public class ScrambleMapTracker
    {
        public Dictionary<string, ScrambleMap> RegisteredMaps { get; set; }

        public ScrambleMapTracker()
        {
            RegisteredMaps = new Dictionary<string, ScrambleMap>();
        }

        public void RegisterMap(string key, ScrambleMap map)
        {
            RegisteredMaps[key] = map;
        }

        public ScrambleMap GetMap(string key)
        {
            if(RegisteredMaps.TryGetValue(key, out var map))
                return map;

            return null;
        }
    }
}