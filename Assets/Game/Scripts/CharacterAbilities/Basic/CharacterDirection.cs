using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DwarfEngine;
using UniRx;
using UniRx.Triggers;

public class CharacterDirection : CharacterAbility
{
    public bool flip;
    public string animatorParamName;
    public ReactiveProperty<Vector2> facingDirection { get; private set; }

    private ReadOnlyReactiveProperty<bool> flipped;
    private bool _flipped;
    
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
        
        if (flip)
        {
            flipped = inputs.look
                .Where(v => v != Vector2.zero)
                .Select((v) =>
                {
                    float angle = Vector2.SignedAngle(Vector2.right, v);
                    return (angle >= 90) || (angle <= -90);
                })
                .ToReadOnlyReactiveProperty(); 
        }
    }

    protected override void SetAnimatorParameters(SpriteModel model)
    {
        model.AddBlendParameter(animatorParamName, facingDirection);
        
        if (flip)
        {
            flipped
                .Where(f => f != _flipped)
                .Subscribe(f =>
                {
                    model.FlipX(f);
                    _flipped = f;
                }); 
        }
    }
}