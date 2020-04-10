using Malee;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    [CreateAssetMenu(fileName = "New SpriteAnimData", menuName = "TouchDevUltimate/Sprite Animations/Sprite Animation Data")]
    public class SpriteAnimationData : ScriptableObject
    {
        public string animName;
        public int framesPerSecond;
        public bool loop;
        public List<Sprite> sprites;
    }

    [Serializable]
    public class SAList : ReorderableArray<SpriteAnimationData>
    {

    }
}
