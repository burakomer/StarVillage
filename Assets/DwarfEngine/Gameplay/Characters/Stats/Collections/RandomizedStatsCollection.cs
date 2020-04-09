using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class RandomizedStatsCollection : StatsCollection
    {
        public List<RandomizedStats<int>> mumberStats;
    }

    [Serializable]
    public class RandomizedStats<T>
    {
        public StatsCollection statsCollection;
        public T minValue;
        public T maxValue;
    }
}
