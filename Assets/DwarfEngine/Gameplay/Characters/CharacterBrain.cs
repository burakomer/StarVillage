using System;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// Brain of a character. Handles input routing. Any state machine related to the character must be done here.
    /// </summary>
    [RequireComponent(typeof(Character))]
    public abstract class CharacterBrain : MonoBehaviour
    {
        public bool active;
        [HideInInspector] public Character character;
        //[HideInInspector] public InputManager inputManager;

        private void Awake()
        {
            SetInputManager();
            PreInit();
        }

        private void Start()
        {
            active = true;
            character = GetComponent<Character>();

            Init();
        }

        private void Update()
        {
            if (active)
            {
                UpdateBrain();
            }
        }

        #region Overwritable Methods

        /// <summary>
        /// Responsible of setting the input source of the brain.
        /// </summary>
        protected abstract void SetInputManager();

        /// <summary>
        /// Called in Awake.
        /// </summary>
        protected virtual void PreInit()
        {

        }

        /// <summary>
        /// Called in Start.
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// Called in Update, if active is true.
        /// </summary>
        protected virtual void UpdateBrain()
        {

        }

        #endregion
    }
}