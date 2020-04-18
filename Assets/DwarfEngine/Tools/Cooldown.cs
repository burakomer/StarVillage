using System;
using UnityEngine;

namespace DwarfEngine
{
    public class Cooldown
    {
        public float coolTime;

        public bool isReady { get; private set; }

        private Action OnReady;
        private float countdown;

        public Cooldown(float _coolTime, Action _OnReady = null)
        {
            coolTime = _coolTime;
            OnReady = _OnReady;
            isReady = true;
        }

        public void Start()
        {
            countdown = coolTime;
            isReady = false;
        }

        private void Stop()
        {
            isReady = true;
            OnReady?.Invoke();
        }

        /// <summary>
        /// Must be called in a MonoBehaviour Update.
        /// </summary>
        public void Update()
        {
            if (!isReady)
            {
                countdown -= Time.deltaTime;

                if (countdown <= 0)
                {
                    countdown = 0;
                    Stop();
                }
            }
        }
    }
}