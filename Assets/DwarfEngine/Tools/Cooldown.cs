using System;
using UnityEngine;

namespace DwarfEngine
{
    public class Cooldown
    {
        public float coolTime;

        public bool isReady { get; private set; }

        private float countdown;

        public Cooldown(float _coolTime)
        {
            coolTime = _coolTime;
        }

        public void Start()
        {
            countdown = coolTime;
            isReady = false;
        }

        public void Stop()
        {
            isReady = true;
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