using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;
using UniRx.Triggers;

public class CharacterMovement : CharacterAbility
{
    public float moveSpeed;

    //private ReactiveProperty<Vector2> movement;
    private ReadOnlyReactiveProperty<bool> moving; 

    protected override void SetInputLogic(CharacterInputs inputs)
    {
        inputs.movement
            .Where(v => v != Vector2.zero)
            .Subscribe(v =>
            {
                Move(v);
            })
            .AddTo(this);

        moving = inputs.movement.
            Select(v => v != Vector2.zero) // True if the movement input is not zero
            .ToReadOnlyReactiveProperty();
    }

    private void Move(Vector2 movementVector)
    {
        transform.position += new Vector3(movementVector.x, movementVector.y) * moveSpeed * Time.deltaTime;
    }

    protected override void SetAnimatorTransitions(SpriteModel model)
    {
        moving
            .Subscribe(isMoving =>
            {
                if (isMoving)
                {
                    model.PlayAnimation("Running");
                }
                else
                {
                    model.PlayAnimation("Idle");
                }
            })
            .AddTo(this);
    }
}
