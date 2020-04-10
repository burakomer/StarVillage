using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CharacterInputs
{
    public IObservable<Vector2> movement;
    public IObservable<Vector2> look;

    public IObservable<bool> attack;
}
