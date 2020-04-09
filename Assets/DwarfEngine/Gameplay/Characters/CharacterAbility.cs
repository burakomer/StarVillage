using System;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class CharacterAbility : MonoBehaviour//, IButtonListener, IJoystickListener, IGestureListener
    {
        public bool active = true;
        [Space]

        protected Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();

            PreInit();
        }

        private void Start()
        {
            // Connect ability input listeners to the brain
            //_character.brain.inputManager.AddListeners(this, this, this);

            Init();
        }

        private void Update()
        {
            if (_character.isAlive)
            {
                if (active)
                {
                    UpdateAbility();
                    if (_character.model.animators.Length > 0 ) UpdateAnimator();
                }
            }
        }

        private void FixedUpdate()
        {
            if (_character.isAlive)
            {
                if (active)
                {
                    UpdateAbilityFixed();
                }
            }
        }

        private void OnDisable()
        {
            //_character.brain.inputManager.RemoveListeners(this, this, this);
        }

        #region Virtual Methods

        protected virtual void PreInit()
        {

        }

        protected virtual void Init()
        {

        }

        protected virtual void UpdateAbility()
        {

        }

        protected virtual void UpdateAbilityFixed()
        {

        }

        protected virtual void UpdateAnimator()
        {

        }

        #endregion
    }
}