using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    //[EquipmentSlot(typeof(Weapon))]
    public class CharacterWeaponEquipmentSlot : CharacterEquipmentSlot<Weapon>, IActiveEquipmentSlot
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
                    _attacking = ((IActiveEquipment)equipment).StartEquipment();
                })
                .AddTo(this);

            inputs.attack
                .Where(t => !t && _attacking)
                .Subscribe(t =>
                {
                    ((IActiveEquipment)equipment).StopEquipment();
                    _attacking = t;
                })
                .AddTo(this);
        }

        public void Use(IActiveEquipment equipment)
        {
            //equipment.StartEquipment();
        }

        protected override void Init()
        {
            base.Init();

            if (equipment != null)
            {
                equipment = Instantiate(equipment, handContainer);
                equipment.SetOwner(_character);
            }
        }

        public override Equipment Equip(Weapon newEquipment)
        {
            Unequip();

            equipment = Instantiate(newEquipment, handContainer);
            return equipment;
        }

        public override void Unequip()
        {
            if (equipment != null)
            {
                Destroy(equipment.gameObject);
            }
        }
    }
}