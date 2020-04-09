using System;
using UnityEngine;

namespace DwarfEngine
{
    public class Timer
    {
        public event Action Elapsed;
        public float interval;
        public bool stopAfterElapsed;
        public bool autoStart;

        public bool isRunning { get; private set; }

        private float timeElapsed;

        public void Start()
        {
            timeElapsed = 0;
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// Must be called in MonoBehaviour Update.
        /// </summary>
        public void Update()
        {
            if (isRunning)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= interval)
                {
                    Elapsed?.Invoke();
                    if (stopAfterElapsed)
                    {
                        Stop();
                    }
                    if (autoStart)
                    {
                        Start();    
                    }
                }
            }
        }
    }
}