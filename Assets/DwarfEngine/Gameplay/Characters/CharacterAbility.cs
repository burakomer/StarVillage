using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// An ability that a character can use. 
    /// Provides easy access to the inputs and the animator of the character.
    /// </summary>
    public abstract class CharacterAbility : MonoBehaviour
    {
        /// <summary>
        /// When set to false, UpdateAbility and UpdateAbilityFixed will not execute.
        /// </summary>
        public bool active = true;

        /// <summary>
        /// Reference to the Character component.
        /// </summary>
        protected Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();

            PreInit();
        }

        private void Start()
        {
            SetInputLogic(_character.brain.inputs);
            SetAnimatorParameters(_character.model);
            SetAnimatorTransitions(_character.model);
            Init();

            this.UpdateAsObservable()
                .Where(_ => _character.isAlive && active)
                .Subscribe(_ =>
                {
                    UpdateAbility();
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => _character.isAlive && active)
                .Subscribe(_ =>
                {
                    UpdateAbilityFixed();
                })
                .AddTo(this);
        }

        #region Virtual Methods

        /// <summary>
        /// Executed in Awake.
        /// </summary>
        protected virtual void PreInit()
        {

        }

        /// <summary>
        /// Executed in Start, after input and animator logic.
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// Wire the inputs to actions here.
        /// </summary>
        protected virtual void SetInputLogic(CharacterInputs inputs)
        {

        }

        /// <summary>
        /// Set animator parameters here. To set transitions, use SetAnimatorTransitions()
        /// </summary>
        protected virtual void SetAnimatorParameters(SpriteModel model)
        {

        }

        /// <summary>
        /// Set animator transitions here. To set parameters, use SetAnimatorParameters()
        /// </summary>
        protected virtual void SetAnimatorTransitions(SpriteModel model)
        {

        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        protected virtual void UpdateAbility()
        {

        }

        /// <summary>
        /// Called a fixed amount of time. Good for physics stuff.
        /// </summary>
        protected virtual void UpdateAbilityFixed()
        {

        }
        #endregion
    }
}