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
        public CharacterInputs inputs;
        [HideInInspector] public Character character;

        private void Awake()
        {
            inputs = new CharacterInputs();
            PreInit();
            SetInputs();
        }

        private void Start()
        {
            active = true;
            character = GetComponent<Character>();

            Init();
        }

        #region Overwritable Methods

        /// <summary>
        /// Responsible of setting the input source of the brain.
        /// </summary>
        protected abstract void SetInputs();

        /// <summary>
        /// Called in Awake. Initialize specific input providers here.
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