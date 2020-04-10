using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SpriteModel : MonoBehaviour
    {
        public SpriteAnimator[] animators;

        private void OnEnable()
        {
            animators = GetComponentsInChildren<SpriteAnimator>(false);
        }

        public void PlayAnimation(string animationName)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.Play(animationName);
            }
        }

        public void PlayAnimation(string blendName, IObservable<Vector2> blendParameter)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.Play(blendName, blendParameter);
            }
        }
    }
}
