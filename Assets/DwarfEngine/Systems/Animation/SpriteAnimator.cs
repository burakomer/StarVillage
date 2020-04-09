using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DwarfEngine
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [Reorderable] public SAList animations;
        public SpriteAnimation currentAnimation;

        private Dictionary<string, int> animationNames;
        private SpriteRenderer _renderer;
        private Coroutine _animationCoroutine;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            animationNames = new Dictionary<string, int>();
            foreach (SpriteAnimation animation in animations)
            {
                animationNames.Add(animation.animationData.animName, animations.IndexOf(animation)); // Add animation name to dictionary for easy access
                animation.events = new Dictionary<int, UnityEvent>(); // Create the event dictionary of the animation
                foreach (SpriteAnimationEvent animationEvent in animation.eventList)
                {
                    animation.events.Add(animationEvent.keyframe, animationEvent.@event); // and populate it with the events from the inspector list
                }
            }

            currentAnimation = animations[0]; // Set the first animation in the list as the entrance animation
            _animationCoroutine = StartCoroutine(PlayCurrentAnimation()); // Start animation
        }

        public void Play(string animationName)
        {
            //Debug.Log((animationNames != null) + " " + gameObject.name);
            if (!animationNames.ContainsKey(animationName))
            {
                return;
            }

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine); 
            }
            currentAnimation = animations[animationNames[animationName]];
            _animationCoroutine = StartCoroutine(PlayCurrentAnimation());
        }

        private IEnumerator PlayCurrentAnimation()
        {
            do
            {
                for (int i = 0; i < currentAnimation.animationData.sprites.Count; i++)
                {
                    _renderer.sprite = currentAnimation.animationData.sprites[i];
                    if (currentAnimation.events.ContainsKey(i))
                    {
                        currentAnimation.events[i]?.Invoke();
                    }
                    yield return new WaitForSeconds(1f / currentAnimation.animationData.framesPerSecond);
                } 
            } while (currentAnimation.animationData.loop);

            _animationCoroutine = null;
        }
    }
}
