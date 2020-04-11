using System;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;

public class CharacterWeaponEquipmentSlot : CharacterEquipmentSlot, IActiveEquipmentSlot
{

    protected override void SetInputLogic(CharacterInputs inputs)
    {
        inputs.attack
            .Where(t => t)
            .Subscribe(_ => Use((IActiveEquipment)equipment))
            .AddTo(this);
    }

    public void Use(IActiveEquipment equipment)
    {
        equipment.StartEquipment();
    }

    protected override void Init()
    {
        
    }
}
