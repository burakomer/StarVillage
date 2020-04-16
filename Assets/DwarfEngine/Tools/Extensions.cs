﻿using UnityEngine;

namespace DwarfEngine
{
    public static class Extensions
    {
        public static void SetSingleton<T>(this MonoBehaviour monoBehaviour, ref T Instance, bool dontDestroyOnLoad = false) where T : MonoBehaviour
        {
            if (dontDestroyOnLoad)
            {
                if (Instance != null)
                {
                    Object.Destroy(monoBehaviour.gameObject);
                    return;
                }

                Instance = monoBehaviour as T;
                Object.DontDestroyOnLoad(monoBehaviour);
            }
            else
            {
                Instance = monoBehaviour as T;
            }
        }

        public static float RoundTo(this float value, int roundTo)
        {
            return Mathf.Round(value * roundTo) / (float) roundTo;
        }

        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}
