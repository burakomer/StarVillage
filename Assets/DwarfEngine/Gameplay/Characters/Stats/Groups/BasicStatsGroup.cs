using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class BasicStatsGroup : StatsGroup
    {
        public NumberStat Strength;
        public NumberStat Vitality;
        public NumberStat Intelligence;
        public NumberStat Wisdom;
        public NumberStat Dexterity;
        public NumberStat Precision;
    }
}