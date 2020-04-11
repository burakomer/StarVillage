using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    public abstract class Weapon : Equipment, IActiveEquipment
    {
        public SpriteAnimator weaponModel;
        public ReactiveProperty<bool> shot;

        [Header("Stats")]
        public float rechargeTime;

        [Header("Animator Parameters")]
        public string attackAnimationName;
        public int attackFrame;
        public int attackEndFrame;

        private Cooldown cooldown;

        private void Start()
        {
            cooldown = new Cooldown(rechargeTime);

            // Sets shot to true when 
            weaponModel.currentAnimation
                .Where(anim => anim.animationName == attackAnimationName && anim.currentFrame == attackFrame)
                .Subscribe(_ =>
                {
                    shot.Value = true;
                })
                .AddTo(this);

            // Stops the weapon when the specified frame is reached
            weaponModel.currentAnimation
                .Where(anim => anim.animationName == attackAnimationName && anim.currentFrame == attackEndFrame)
                .Subscribe(_ =>
                {
                    shot.Value = false;
                    StopEquipment();
                })
                .AddTo(this);

            shot
                .Where(b => b)
                .Subscribe(_ =>
                {
                    Shoot();
                })
                .AddTo(this);
        }

        protected abstract void Shoot();

        public void StartEquipment()
        {
            // TODO : Check cooldown
        }

        public void StopEquipment()
        {
            
        }

        public override void EquipLogic()
        {
            
        }

        public override void UnequipLogic()
        {

        }
    }
}
