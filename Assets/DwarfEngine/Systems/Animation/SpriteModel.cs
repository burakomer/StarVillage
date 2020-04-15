using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SpriteModel : MonoBehaviour
    {
        public string animatorTag; // Only get the animators that have this tag
        public List<SpriteAnimator> animators;

        protected virtual void OnEnable()
        {
            animators = new List<SpriteAnimator>();
            foreach(SpriteAnimator animator in GetComponentsInChildren<SpriteAnimator>(false))
            {
                if (string.IsNullOrEmpty(animatorTag) || !animator.CompareTag(animatorTag))
                {
                    return;
                }
                animators.Add(animator);
            }
        }

        public virtual void PlayAnimation(string animationName)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.Play(animationName);
            }
        }

        public virtual void AddBlendParameter(string parameterName, IObservable<Vector2> parameter)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.AddBlendParameter(parameterName, parameter);
            }
        }

        public virtual void FlipX(bool flipped)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.FlipX(flipped);
            }
        }
    }
}
