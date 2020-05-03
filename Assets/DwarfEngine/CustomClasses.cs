using Malee;
using System;
using UnityEngine;

namespace DwarfEngine
{
    [Serializable]
    public class StringList : ReorderableArray<string> { }
    
    [Serializable]
    public class SANList : ReorderableArray<SpriteAnimatorNode> { }

    //[Serializable]
    //public class StatList : ReorderableArray<Stat> { }

    //[Serializable]
    //public class NumberStatList : ReorderableArray<NumberStat> { }
}
