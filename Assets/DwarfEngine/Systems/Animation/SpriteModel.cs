using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SpriteModel : MonoBehaviour
    {
        public string animatorTag; // Only get the animators that have this tag
        public List<SpriteAnimator> animators;

        private void OnEnable()
        {
            animators = new List<SpriteAnimator>();
            foreach(SpriteAnimator animator in GetComponentsInChildren<SpriteAnimator>(false))
            {
                if (string.IsNullOrEmpty(animatorTag) && !animator.CompareTag(animatorTag))
                {
                    return;
                }
                animators.Add(animator);
            }
        }

        public void PlayAnimation(string animationName)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.Play(animationName);
            }
        }

        public void AddBlendParameter(string parameterName, IObservable<Vector2> parameter)
        {
            foreach (SpriteAnimator animator in animators)
            {
                animator.AddBlendParameter(parameterName, parameter);
            }
        }
    }
}
