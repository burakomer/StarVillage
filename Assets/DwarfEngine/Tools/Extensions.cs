using Unity.Mathematics;
using UnityEngine;

namespace DwarfEngine
{
    public static class Extensions
    {
        /// <summary>
        /// Used to make a MonoBehaviour singleton.
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="Instance">The static field where the singleton will exist.</param>
        /// <param name="dontDestroyOnLoad">If set to true, the object will persist between scenes. Defaults to false.</param>
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

        #region Vector Extensions
        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 ToVector3(this float value, float x, float y, float z)
        {
            return new Vector3(x, y, z) * value;
        }

        public static Vector3 ToVector3(this Vector2Int vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static float2 ToFloat2(this Vector2 vector)
        {
            return new float2(vector.x, vector.y);
        }

        public static float2 ToFloat2(this Vector3 vector)
        {
            return new float2(vector.x, vector.y);
        }
        #endregion
    }
}
