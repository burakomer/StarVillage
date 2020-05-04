using System;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// Functions as a generalized place for important game assets. Must be initialized at the start of the game.
    /// </summary>
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets Instance;

        [Header("Assets")] 

        public FilledProgressBar GenericHealthBar;
        public TextPopup GenericTextPopup;

        #region Custom Assets

        // Continue adding assets needed in the project here.

        #endregion

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}