using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;

public class CharacterMovement : CharacterAbility
{
    private Vector3 movement;

    protected void Start()
    {
        _character.brain.inputs.movement
            .Where(v => v != Vector2.zero)
            .Subscribe(v =>
            {
                movement = new Vector3(v.x, v.y) * 10f * Time.deltaTime;
                transform.position += movement;
            })
            .AddTo(this);

        _character.brain.inputs.look
            .Where(v => v != Vector2.zero)
            .Subscribe(v =>
            {
                transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, v)); 
            })
            .AddTo(this);

        _character.brain.inputs.attack
            .Where(t => t)
            .Subscribe(_ => Debug.Log("Attack button pressed!"))
            .AddTo(this);
    }
}
