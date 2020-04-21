using System;
using UnityEngine;

namespace DwarfEngine
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public Character player;

        private void Awake()
        {
            this.SetSingleton(ref Instance);

            player = Instantiate(player, Vector3.zero, Quaternion.identity);
        }
    }
}