using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class BaseSpriteAnimatorNode : ScriptableObject
    {
        public string nodeName;
    }

    [Serializable]
    public class SANList : ReorderableArray<BaseSpriteAnimatorNode>
    {

    }
}