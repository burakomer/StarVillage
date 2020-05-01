using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    //[EquipmentSlot(typeof(Weapon))]
    public class CharacterWeaponSlot : CharacterEquipmentSlot<Weapon>
    {
        public Transform leftHand;
        public Transform rightHand;
        public Transform handContainer;

        private bool _attacking;

        protected override void SetInputLogic(CharacterInputs inputs)
        {
            inputs.attack
                .Where(t => t && !_attacking)
                .Subscribe(t =>
                {
                    if (Equipment != null)
                    {
                        _attacking = ((IActiveEquipment)Equipment).StartEquipment(); 
                    }
                })
                .AddTo(this);

            inputs.attack
                .Where(t => !t && _attacking)
                .Subscribe(t =>
                {
                    if (Equipment != null)
                    {
                        ((IActiveEquipment)Equipment).StopEquipment();
                    }
                    _attacking = t; 
                })
                .AddTo(this);
        }

        protected override void Init()
        {
            base.Init();

            if (Equipment != null)
            {
                Equipment = Instantiate(Equipment, handContainer);
                Equipment.SetOwner(_character);
            }
        }

        public override Equipment Equip(Weapon newEquipment)
        {
            Unequip();

            Equipment = Instantiate(newEquipment, handContainer);
            return Equipment;
        }

        public override void Unequip()
        {
            if (Equipment != null)
            {
                Destroy(Equipment.gameObject);
                Equipment = null;
            }
        }
    }
}