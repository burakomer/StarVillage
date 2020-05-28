using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;


public class PlayerWeaponSwitch : MonoBehaviour
{
    PlayerBrain brain;

    public WeaponItemAsset bow;
    public WeaponItemAsset sword;
    private int currentWeapon;

    void Start()
    {
        brain = GetComponent<PlayerBrain>();
        brain.pinput.weaponSwitch.Where(button => button).Subscribe(_ => {

            if (currentWeapon == 1)
                currentWeapon = 0;
            else
                currentWeapon = 1;

            if (currentWeapon == 1)
            {
                sword.Equip(brain.character.equipmentManager, 0);

            }
            else
            {
                bow.Equip(brain.character.equipmentManager, 0);

            }

        }).AddTo(this);
    }
    
    void Update()
    {
        
    }
}
