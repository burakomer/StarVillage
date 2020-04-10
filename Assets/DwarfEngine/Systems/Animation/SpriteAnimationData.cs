using Malee;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    [CreateAssetMenu(fileName = "New SpriteAnimData", menuName = "Sprite Animations/Sprite Animation Data")]
    public class SpriteAnimationData : BaseSpriteAnimatorNode
    {
        public int framesPerSecond;
        public bool loop;
        public List<Sprite> sprites;
    }
}
