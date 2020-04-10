using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;

public class CharacterMovement : CharacterAbility
{
    private Vector3 movement;

    protected override void SetInputLogic(CharacterInputs inputs)
    {
        inputs.movement
            .Where(v => v != Vector2.zero)
            .Subscribe(v =>
            {
                movement = new Vector3(v.x, v.y) * 10f * Time.deltaTime;
                transform.position += movement;
            })
            .AddTo(this);

        inputs.attack
            .Where(t => t)
            .Subscribe(_ => Debug.Log("Attack button pressed!"))
            .AddTo(this);
    }
}
