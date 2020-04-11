﻿using Malee;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DwarfEngine
{
    [Serializable]
    public class StringList : ReorderableArray<string> { }
    
    [Serializable]
    public class SANList : ReorderableArray<SpriteAnimatorNode> { }
}
