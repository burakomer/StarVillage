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
        public bool active;
        [Space]

        public SpriteAnimatorData animatorData;

        /// <summary>
        /// Provides the current playing animation name and the frame it's on. Enables executing actions at specific animations and frames.
        /// </summary>
        public ReactiveProperty<(string animationName, int currentFrame)> currentAnimation { get; private set; }

        private Dictionary<string, SpriteAnimationData> animationDatas;
        private Dictionary<string, SpriteBlendData> blendDatas;
        private Dictionary<string, IObservable<Vector2>> blendParameterDictionary;

        private SpriteAnimatorNode currentNode;

        private SpriteAnimationData currentAnimationData;
        private SpriteRenderer _renderer;
        
        private IDisposable animationDisposable;
        private IDisposable blendDisposable;

        private void Awake()
        {
            if (!active)
            {
                return;
            }

            _renderer = GetComponent<SpriteRenderer>();

            animationDatas = new Dictionary<string, SpriteAnimationData>();
            blendDatas = new Dictionary<string, SpriteBlendData>();
            blendParameterDictionary = new Dictionary<string, IObservable<Vector2>>();

            foreach (SpriteAnimatorNode animatorNode in animatorData.animatorNodes)
            {
                if (animatorNode.node is SpriteBlendData)
                {
                    blendDatas.Add(animatorNode.node.nodeName, (SpriteBlendData)animatorNode.node);
                    foreach (SpriteBlendNode blendNode in ((SpriteBlendData)animatorNode.node).blendSpace)
                    {
                        animationDatas.Add(blendNode.animationData.nodeName, blendNode.animationData);
                    }
                }
                else if (animatorNode.node is SpriteAnimationData)
                {
                    // Add animation name to dictionary for easy access
                    animationDatas.Add(animatorNode.node.nodeName, (SpriteAnimationData)animatorNode.node); 
                }
            }

            foreach (string parameterName in animatorData.blendParameters)
            {
                blendParameterDictionary.Add(parameterName, null);
            }

            currentAnimation = new ReactiveProperty<(string animationName, int currentFrame)>((string.Empty, 0));
            currentNode = animatorData.animatorNodes[0]; // Set the first node as default node

            //Play(currentNode.node.nodeName);
        }

        public void Play(string nodeName)
        {
            if (currentNode != null)
            {
                if (currentNode != animatorData.animatorNodes[0] && !currentNode.transitions.Contains(nodeName)) // If the current node isn't the default node and current node doesn't have the correct transition
                {
                    Debug.LogError("Transition to " + nodeName + " from " + currentNode.node.nodeName + " doesn't exist!");
                    return;
                }
            }

            if (blendDatas.ContainsKey(nodeName))
            {
                Play(nodeName, blendDatas[nodeName].parameterName);
            }
            else if (animationDatas.ContainsKey(nodeName))
            {
                Play(nodeName, 0);
            }
        }

        /// <summary>
        /// Start playing a blend space.
        /// </summary>
        private void Play(string blendName, string blendParameter)
        {
            if (!blendDatas.ContainsKey(blendName))
            {
                Debug.LogError("Invalid blend name!");
                return;
            }

            if (!blendParameterDictionary.ContainsKey(blendParameter))
            {
                Debug.LogError("Invalid blend parameter!");
                return;
            }

            if (blendParameterDictionary[blendParameter] == null)
            {
                Debug.LogError("Observable Vector2 not initialized!");
                return;
            }
            
            if (blendDisposable != null)
            {
                blendDisposable.Dispose();
            }

            currentNode = animatorData.animatorNodes.Find(n => n.node.nodeName == blendName);

            SpriteBlendData blendData = blendDatas[blendName];
            blendDisposable = blendParameterDictionary[blendParameter]
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

        /// <summary>
        /// Play an animation
        /// </summary>
        private void Play(string animationName, int startingFrame = 0, bool isBlendAnimation = false)
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

            if (!isBlendAnimation)
            {
                if (blendDisposable != null)
                {
                    blendDisposable.Dispose();
                }
                currentNode = animatorData.animatorNodes.Find(n => n.node.nodeName == animationName);
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

        public void AddBlendParameter(string parameterName, IObservable<Vector2> parameter)
        {
            if (blendParameterDictionary.ContainsKey(parameterName))
            {
                blendParameterDictionary[parameterName] = parameter;
            }
            else
            {
                blendParameterDictionary.Add(parameterName, parameter);
            }
        }

        public void FlipX(bool flipped)
        {
            transform.rotation = Quaternion.Euler(0f, flipped ? 180f : 0f, 0f);
        }
    }

    public enum AnimatorNodeType { SingleAnimation, BlendSpace }
}
