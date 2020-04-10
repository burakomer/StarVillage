using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

namespace DwarfEngine
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [Reorderable] public SAList animations;

        /// <summary>
        /// Provides the current playing animation name and the frame it's on. Enables executing actions at specific animations and frames.
        /// </summary>
        public Subject<(string animationName, int currentFrame)> currentAnimation { get; private set; }

        private Dictionary<string, int> animationNames;
        private SpriteAnimationData currentAnimationData;
        private SpriteRenderer _renderer;
        
        private IDisposable animationDisposable;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();

            animationNames = new Dictionary<string, int>();
            foreach (SpriteAnimationData animation in animations)
            {
                // Add animation name to dictionary for easy access
                animationNames.Add(animation.animName, animations.IndexOf(animation)); 
            }

            currentAnimationData = animations[0]; // Set the first animation in the list as the entrance animation
            currentAnimation = new Subject<(string animationName, int currentFrame)>();
            Play(0);
        }

        public void Play(string animationName)
        {
            if (!animationNames.ContainsKey(animationName))
            {
                Debug.LogError("Invalid animation name");
                return;
            }

            if (animationDisposable != null)
            {
                animationDisposable.Dispose(); 
            }

            currentAnimationData = animations[animationNames[animationName]];

            animationDisposable = Observable
                .FromCoroutine<(string animationName, int currentFrame)>(observer => PlayCurrentAnimation(observer))
                .Subscribe(currentAnimation)
                .AddTo(this);
        }

        public void Play(int animationIndex)
        {
            if (animationIndex < 0 || animationIndex >= animations.Length)
            {
                Debug.LogError("Invalid animation index");
                return;
            }

            Play(animations[animationIndex].animName);
        }

        private IEnumerator PlayCurrentAnimation(IObserver<(string animationName, int currentFrame)> observer)
        {
            do
            {
                for (int i = 0; i < currentAnimationData.sprites.Count; i++)
                {
                    _renderer.sprite = currentAnimationData.sprites[i];
                    observer.OnNext((currentAnimationData.animName, i));
                    
                    yield return new WaitForSeconds(1f / currentAnimationData.framesPerSecond);
                } 
            } while (currentAnimationData.loop);

            observer.OnCompleted();
        }
    }
}
