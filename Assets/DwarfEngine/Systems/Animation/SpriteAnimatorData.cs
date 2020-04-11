using Malee;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [CreateAssetMenu(fileName = "New Sprite Animator Data", menuName = "Sprite Animations/Sprite Animator Data")]
    public class SpriteAnimatorData : ScriptableObject
    {
        public List<SpriteAnimatorNode> animatorNodes;
        [Reorderable] public StringList blendParameters;
    }
}
