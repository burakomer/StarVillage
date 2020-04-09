using System;
using UnityEngine;

namespace DwarfEngine.AI
{
    public abstract class BTBaseNode : ScriptableObject
    {
        public abstract BTNodeState Tick(GameObject owner);
    }
}