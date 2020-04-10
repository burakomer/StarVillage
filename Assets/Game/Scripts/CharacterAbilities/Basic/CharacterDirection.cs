using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;
using UniRx.Triggers;

public class CharacterDirection : CharacterAbility
{
    public ReactiveProperty<Vector2> facingDirection { get; private set; }

    protected override void SetInputLogic(CharacterInputs inputs)
    {
        facingDirection = new ReactiveProperty<Vector2>();
        inputs.look
            .Where(v => v != Vector2.zero)
            .Subscribe(v =>
            {
                facingDirection.Value = v;
            })
            .AddTo(this);
    }

    protected override void SetAnimatorLogic(SpriteModel model)
    {
        model.PlayAnimation("Running", facingDirection);
    }
}