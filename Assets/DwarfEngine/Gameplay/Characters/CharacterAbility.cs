using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class CharacterAbility : MonoBehaviour
    {
        public bool active = true;

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
                .Where(_ => _character.isAlive)
                .Subscribe(_ =>
                {
                    UpdateAbility();
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => _character.isAlive)
                .Subscribe(_ =>
                {
                    UpdateAbilityFixed();
                })
                .AddTo(this);
        }

        #region Virtual Methods

        protected virtual void PreInit()
        {

        }

        protected virtual void Init()
        {

        }

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

        protected virtual void UpdateAbility()
        {

        }

        protected virtual void UpdateAbilityFixed()
        {

        }
        #endregion
    }
}