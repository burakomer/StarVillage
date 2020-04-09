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
    public class SpriteAnimation
    {
        public SpriteAnimationData animationData;
        public List<SpriteAnimationEvent> eventList;

        public Dictionary<int, UnityEvent> events;
    }

    [Serializable]
    public class SpriteAnimationEvent
    {
        public int keyframe;
        public UnityEvent @event;
    }

    [Serializable]
    public class SAList : ReorderableArray<SpriteAnimation>
    {

    }
}
