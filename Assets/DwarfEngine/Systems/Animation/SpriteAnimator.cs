using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [Reorderable] public SANList animatorNodes;

        /// <summary>
        /// Provides the current playing animation name and the frame it's on. Enables executing actions at specific animations and frames.
        /// </summary>
        public ReactiveProperty<(string animationName, int currentFrame)> currentAnimation { get; private set; }

        private Dictionary<string, SpriteAnimationData> animationDatas;
        private Dictionary<string, SpriteBlendData> blendDatas;
        private SpriteAnimationData currentAnimationData;
        private SpriteRenderer _renderer;
        
        private IDisposable animationDisposable;
        private IDisposable blendDisposable;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            animationDatas = new Dictionary<string, SpriteAnimationData>();
            blendDatas = new Dictionary<string, SpriteBlendData>();

            foreach (BaseSpriteAnimatorNode animatorNode in animatorNodes)
            {
                if (animatorNode is SpriteBlendData)
                {
                    blendDatas.Add(animatorNode.nodeName, (SpriteBlendData)animatorNode);
                    foreach (SpriteBlendNode blendNode in ((SpriteBlendData)animatorNode).blendSpace)
                    {
                        animationDatas.Add(blendNode.animationData.nodeName, blendNode.animationData);
                    }
                }
                else if (animatorNode is SpriteAnimationData)
                {
                    // Add animation name to dictionary for easy access
                    animationDatas.Add(animatorNode.nodeName, (SpriteAnimationData)animatorNode); 
                }
            }

            currentAnimation = new ReactiveProperty<(string animationName, int currentFrame)>((string.Empty, 0));
            
            //currentAnimationData = animations[0]; // Set the first animation in the list as the entrance animation
            //Play(0);
        }

        /// <summary>
        /// Select a blend space to play.
        /// </summary>
        /// <param name="blendName">Name of the blend space.</param>
        /// <param name="blendParameter">Parameter to use for blending.</param>
        public void Play(string blendName, IObservable<Vector2> blendParameter)
        {
            if (!blendDatas.ContainsKey(blendName))
            {
                Debug.LogError("Invalid blend name!");
                return;
            }

            if (blendDisposable != null)
            {
                blendDisposable.Dispose();
            }

            SpriteBlendData blendData = blendDatas[blendName];
            blendDisposable = blendParameter
                .Subscribe(v =>
                {
                    ProcessBlend(blendData, v);
                })
                .AddTo(this);
        }

        private void ProcessBlend(SpriteBlendData blendData, Vector2 blendParameter)
        {
            (string animationName, float minDistance) nextTransition = (string.Empty, 10f);
            
            foreach (SpriteBlendNode animation in blendData.blendSpace)
            {
                float checkDistance = Vector2.Distance(blendParameter, animation.blendPosition);
                if (checkDistance < nextTransition.minDistance)
                {
                    nextTransition = (animation.animationData.nodeName, checkDistance);
                }
            }

            if (currentAnimationData != null)
            {
                if (nextTransition.animationName != currentAnimationData.nodeName) // If it's not already playing
                {
                    Play(nextTransition.animationName, currentAnimation.Value.currentFrame + 1, true); // Pass the next frame of the current animation
                }
            }
            else
            {
                Play(nextTransition.animationName, 0, true);
            }
        }

        public void Play(int animationIndex)
        {
            if (animationIndex < 0 || animationIndex >= animatorNodes.Length)
            {
                Debug.LogError("Invalid animation index");
                return;
            }

            Play(animatorNodes[animationIndex].nodeName);
        }

        public void Play(string animationName, int startingFrame = 0, bool isBlendAnimation = false)
        {
            if (!animationDatas.ContainsKey(animationName))
            {
                Debug.LogError("Invalid animation name");
                return;
            }

            #region Disposing
            if (animationDisposable != null)
            {
                animationDisposable.Dispose();
            }

            if (!isBlendAnimation && blendDisposable != null)
            {
                blendDisposable.Dispose();
            } 
            #endregion

            currentAnimationData = animationDatas[animationName]; // Set the current animation data to the new one

            animationDisposable = Observable // Create an observable from coroutine
                .FromCoroutine<(string animationName, int currentFrame)>(observer => PlayCurrentAnimation(observer, startingFrame))
                .Subscribe(t =>
                {
                    currentAnimation.Value = t;
                })
                .AddTo(this);
        }

        private IEnumerator PlayCurrentAnimation(IObserver<(string animationName, int currentFrame)> observer, int startingFrame)
        {
            int currentFrame = startingFrame; // Initial frame
            do
            {
                do
                {
                    if (currentFrame >= currentAnimationData.sprites.Count)
                    {
                        currentFrame = 0;
                    }

                    _renderer.sprite = currentAnimationData.sprites[currentFrame];
                    observer.OnNext((currentAnimationData.nodeName, currentFrame)); // Notify the observers with the new current frame

                    yield return new WaitForSeconds(1f / currentAnimationData.framesPerSecond); // Wait
                    currentFrame++; // Increment the current frame
                } while (currentFrame < currentAnimationData.sprites.Count);
            } while (currentAnimationData.loop);

            observer.OnCompleted();
        }
    }

    public enum AnimatorNodeType { SingleAnimation, BlendSpace }
}
