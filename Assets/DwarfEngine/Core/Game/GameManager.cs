using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DwarfEngine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public event Action OnSceneLoaded;
        public event Action OnSaveInitiated;
        public event Action OnLoadInitiated;

        public string currentScene;

        #region Unity Methods
        private void Awake()
        {
            this.SetSingleton(ref Instance, true);

            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }
        #endregion

        #region Event Callers
        public void LoadInitiate()
        {
            OnLoadInitiated?.Invoke();
        }

        public void SaveInitiate()
        {
            OnSaveInitiated?.Invoke();
        } 
        #endregion
        
        #region Time Management

        #endregion

        #region Scene Management
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            currentScene = SceneManager.GetActiveScene().name;

            OnSceneLoaded?.Invoke();
            LoadInitiate();
        } 
        #endregion
    }
}