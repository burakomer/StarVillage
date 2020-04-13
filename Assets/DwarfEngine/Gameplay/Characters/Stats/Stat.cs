using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [Serializable]
    public class Stat
    {
        public string name;
        //public StatModifierType type;
        public float value;
    }

    public enum StatModifierType { Flat, Percentage }
}
