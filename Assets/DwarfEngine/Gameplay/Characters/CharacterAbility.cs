using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DwarfEngine
{
    public abstract class CharacterAbility : MonoBehaviour
    {
        public bool active = true;
        [Space]

        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();

            PreInit();
        }

        private void Start()
        {
            SetInputLogic(_character.brain.inputs);
            SetAnimatorLogic(_character.model);
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

        protected virtual void SetAnimatorLogic(SpriteModel model)
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